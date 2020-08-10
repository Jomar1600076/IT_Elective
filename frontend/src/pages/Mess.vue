<template>
  <q-page 
    ref="pageChat"
    class="flex column">
    <q-banner inline-actions 
    class="text-white text-center bg-red-4">
      {{otherUser}}
    </q-banner>
    <div 
      :class="{ 'invisible' : !showMessages }"
      class="q-pa-md column col justify-end">
      <q-chat-message
        v-for="message in messages" 
        :key="message.id"
        :name="message.sender"
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
import * as signalR from '@aspnet/signalr'

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:3000/chathub", {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    })
    .build()

export default {
  data(){
    return {
      newMessage : '',
      showMessages: false
    }
  },
  computed : {
    ...mapState({
          account: state => state.account,
          messages: state => state.users.messages.messages,
          accountName : state => state.account.user.firstName,
          otherUser: state => state.users.otherUser
      }),
  },
  methods: {
    ...mapActions('users', ['getMessages','sendMessages', 'getOtherUser']),
    sendMessage(){
      this.sendMessages({
        message: this.newMessage,
        reciever : this.$route.params.otherUserId,
        sender : this.$store.state.account.user.id
      })
      this.clearMessage()
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
    this.getMessages(this.$route.params.otherUserId);
    this.getOtherUser(this.$route.params.otherUserId);
  } 
}
</script>
