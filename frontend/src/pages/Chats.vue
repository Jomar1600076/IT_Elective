<template>
  <q-page class=" flex q-pa-md">
    <q-list
    class="full-width"
    separator>
      <q-item v-for="user in users" 
        :key="user.convoId"
        :to="/messages/+ user.convoId" clickable v-ripple>
        <q-item-section avatar>
          <q-avatar color="primary" text-color="white">
            {{ user.firstName.charAt(0) }}
          </q-avatar>
        </q-item-section>

        <q-item-section>
          <q-item-label>{{ user.firstName + ' '+ user.lastName }}</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
    <q-page-sticky position="bottom-right" :offset="[18, 18]">
      <q-btn fab icon="add" color="primary"
      @click="bar = true" />
    </q-page-sticky>

    <q-dialog v-model="bar">
      <q-card>
        <q-card-section>
          <div class="text-h6">Add Convo</div>
        </q-card-section>
        <q-separator />
        <q-card-section style="max-height: 50vh" class="scroll">
          <q-list bordered class="rounded-borders" style="max-width: 600px" separator>
            <q-item v-for="user in allUsers"
            :key="user.index"> 
              <q-item-section top>
                <q-item-label lines="1" class="q-mt-xs text-body2">
                  <span class="cursor-pointer">{{ user.firstName + ' '+ user.lastName }}</span>
                </q-item-label>
              </q-item-section>
              <q-item-section top side>
                <div class="text-grey-8 q-gutter-xs">
                  <q-btn size="12px" flat dense round icon="add" @click="newConvo(user.id)" />
                </div>
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>
        <q-separator />
        <q-card-actions align="right">
          <q-btn flat label="Close" color="primary" v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { mapState, mapActions } from 'vuex'
let userToken = JSON.parse(localStorage.getItem('user'));
export default {
  data(){
    return {
      bar: false,
      userID: ''
    }
  },
  computed: {
    ...mapState({
      allUsers : state => state.users.allUsers.allusers,
      users : state => state.users.convo.convo,
      currentUser : state => state.account.user.id
    })
  },
  created (){
    this.getAllUsers()
    this.getUsers()
  },
  methods :{
    ...mapActions('users',{
      getAllUsers : 'getConvo',
      getUsers : 'getAll'
    }),
      newConvo(reciever){
        this.addConvo({
          sUser : reciever
        })
      },
      addConvo(sUser){
        this.$axios({
        method: 'post',
        url: 'http://localhost:3000/message/addconvo',
        data: JSON.stringify(sUser),
        headers: {
            Authorization : 'Bearer '+ userToken.token,
            'Content-Type': 'application/json' 
            }
        }).then((res => {
          console.log(res.data);
          this.bar = false
          this.$router.go()
        }))
        .catch(error => {console.log( 'the error has occured: ' + error) })
    }
  }
}
</script>
