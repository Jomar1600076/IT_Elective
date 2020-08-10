<template>
    <q-form @submit="submitform">
        <q-input 
            v-if="tab == 'register'"
            color="grey-3" 
            label-color="primary" 
            outlined 
            class="q-mb-sm"
            v-model="user.firstname" 
            label="First Name">
            <template v-slot:append>
                <q-icon name="person" color="primary" />
            </template>
        </q-input>
        <q-input 
            v-if="tab == 'register'"
            color="grey-3" 
            label-color="primary" 
            outlined 
            class="q-mb-sm"
            v-model="user.lastname" 
            label="Last Name">
            <template v-slot:append>
                <q-icon name="person" color="primary" />
            </template>
        </q-input>
        <q-input 
            color="grey-3" 
            label-color="primary" 
            outlined 
            class="q-mb-sm"
            v-model="user.email" 
            label="Email"
            type="email">
            <template v-slot:append>
                <q-icon name="mail" color="primary" />
            </template>
        </q-input>
        <q-input 
            color="grey-3" 
            label-color="primary" 
            outlined 
            class="q-mb-sm"
            v-model="user.password" 
            label="Password"
            type="password">
            <template v-slot:append>
                <q-icon name="lock" color="primary" />
            </template>
        </q-input>
        <div class="row">
            <q-space/>
            <q-btn 
            type="submit"
            color="primary"
            :label="tab" />
        </div>
    </q-form>
</template>
<script>
import { mapActions } from 'vuex'
export default {
    props : ["tab"],
    data(){
        return {
            user: {
                firstname: '',
                lastname: '',
                email: 'john@test.com',
                password: 'john123'
            }
        }
    },
    methods : {
        ...mapActions('account', ['register', 'login']),
        submitform(){
            if(this.tab =='login'){
                this.login(this.user)
            }else{
                this.register(this.user)
            }
        }
    }
}
</script>