import * as signalR from '@microsoft/signalr'

export default {
  install (Vue) {
    // use a new Vue instance as the interface for Vue components to receive/send SignalR events
    // this way every component can listen to events or send new events using this.$questionHub
    const messageHub = new Vue()

    // Provide methods to connect/disconnect from the SignalR hub
    let connection = null
    let startedPromise = null
    let manuallyClosed = false

    Vue.prototype.startSignalR = () => {

      const connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:3000/chatHub", {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        }).build()
      connection.start()


      // Forward hub events through the event, so we can listen for them in the Vue components
      connection.on('UserAdded', (user) => {
        messageHub.$emit('user-added', user)
      })
      connection.on('ChatMessage', message => {
        messageHub.$emit('message-added', message)
      })
      // You need to call connection.start() to establish the connection but the client wont handle reconnecting for you!
      // Docs recommend listening onclose and handling it there.
      // This is the simplest of the strategies
      function start () {
        startedPromise = connection.start()
          .then(console.log('Connected to Hub'))
          .catch(err => {
            console.error('Failed to connect with hub', err)
            return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
          })
        return startedPromise
      }
      connection.onclose(() => {
        if (!manuallyClosed) start()
      })

      // Start everything
      manuallyClosed = false
      start()
    }
    Vue.prototype.stopSignalR = () => {
      if (!startedPromise) return

      manuallyClosed = true
      return startedPromise
        .then(() => connection.stop())
        .then(() => { startedPromise = null })
    }
      messageHub.sendMessage = (message) => {
      if (!startedPromise) return

      return startedPromise
        .then(() => connection.invoke('SendMessage', message))
        .catch(console.error)
    }
  }
}
