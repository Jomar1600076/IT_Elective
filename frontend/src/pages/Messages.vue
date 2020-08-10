<template>
  <q-page 
    ref="pageChat"
    class="flex column">
    <div 
      :class="{ 'invisible' : !showMessages }"
      class="q-pa-md column col justify-end">
      <q-chat-message
        v-for="message in messages" 
        :key="message.id"
        :name="message.senderName"
        :text="[message.message]"   
        :sent="message.sender == accountName ? true : false"
        :when="message.message_time"
      />
    </div>
    <q-footer elevated>
      <q-toolbar>
        <q-form
          @submit="sendMessage"
          class="full-width">     
          <q-input 
            ref="newMessage"
            v-model="newMessage"
            bg-color="white"
            outlined 
            label="Message. . . " dense>
            <template v-slot:after>
              <q-btn 
              round 
              dense 
              flat
              icon="send" 
              color="white"/>
            </template>
          </q-input>
        </q-form>
      </q-toolbar>
    </q-footer>

  </q-page>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import * as signalR from '@microsoft/signalr'

let userToken = JSON.parse(localStorage.getItem('user'));
let connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:3000/chatHub", {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
    }).build()

export default {
  data(){
    return {
      connection: null,
      newMessage : '',
      showMessages: false,
      messages: []
    }
  },

  computed : {
    ...mapState({
          account: state => state.account,
          accountName : state => state.account.user.id,
          otherUser: state => state.users.otherUser
      }),
  },  
  methods: {
    ...mapActions('users', ['getOtherUser']),
    sendMessage(){  
      this.sendMessages({
        message: this.newMessage,
        sender : this.$store.state.account.user.id
      })
    },
    sendMessages(message, sender){
        this.$axios({
        method: 'post',
        url: 'http://localhost:3000/message/addmessage/'+ this.$route.params.otherUserId,
        data: JSON.stringify(message, sender),
        headers: {
            Authorization : 'Bearer '+ userToken.token,
            'Content-Type': 'application/json' 
          }
        })
          .then((response) => {
            this.clearMessage()
          }) 
          .catch(error => {console.log( 'the error has occured: ' + error) })
    },
    clearMessage(){
      this.newMessage = ''
      this.$refs.newMessage.focus()
    },

    scrollTobottom(){
      let pageChat = this.$refs.pageChat.$el
      setTimeout(() => {
         window.scrollTo(0, pageChat.scrollHeight)
      }, 20);
    }
  },

  watch: {
    messages: function(val) {
        if (Object.keys(val).length){
          this.scrollTobottom()
          setTimeout(() => {
            this.showMessages = true
          }, 200);
        }
    }
  },
  mounted(){
    this.getOtherUser(this.$route.params.otherUserId);
  },
  created(){
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.on('MessageAdded', message => {
          this.messages.push(message)
        })
    } else {
    connection.start()
        .then(() => 
        connection.on('MessageAdded', message => {
          this.messages.push(message)
        })
      )
    }
    this.$axios({
          method: 'get',
          url: 'http://localhost:3000/message/getmessage/' + this.$route.params.otherUserId,
          headers: {
              Authorization : 'Bearer '+ userToken.token
            }
          }).then((res) => {
                if (connection.state === signalR.HubConnectionState.Connected) {
                  connection.invoke('JoinConvo', this.$route.params.otherUserId)
                } else {
                  connection.start()
                    .then(() => 
                      connection.invoke('JoinConvo', this.$route.params.otherUserId)
                  )
                }
            this.messages = res.data
          }).catch(error => {console.log( 'the error has occured: ' + error) })
    }
  }
</script>
