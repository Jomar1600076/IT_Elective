<template>
  <div class="q-pa-md full-width">
    <q-list bordered class="rounded-borders" style="max-width: 600px" separator>
      <q-item-label header>Google Inbox style</q-item-label>
      <q-item v-for="user in users"
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
  </div>
</template>
<script>
import { mapActions, mapState } from 'vuex'
let userToken = JSON.parse(localStorage.getItem('user'));

export default {
    data(){
        return {
        }
    },
    computed: {
    ...mapState({
        users : state => state.users.allUsers.allusers,
        currentUser : state => state.account.user.id
        })
    },
    methods : {
        ...mapActions('users',{
            getAllUsers : 'getAll'
        }),
        newConvo(index){
            alert(index)
        },
        addConvo(reciever){
            this.$axios({
            method: 'post',
            url: 'http://localhost:3000/message/addconvo',
            data: JSON.stringify(reciever),
            headers: {
                Authorization : 'Bearer '+ userToken.token,
                'Content-Type': 'application/json' 
                }
            })
            .catch(error => {console.log( 'the error has occured: ' + error) })
        }
    },
    created (){
        this.getAllUsers()
    }
}
</script>