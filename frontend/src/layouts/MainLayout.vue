<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          v-if="$route.fullPath.includes('/messages') || $route.fullPath.includes('/users')"
          v-go-back.single
          icon="arrow_back" 
          label="Back"
          flat/>
        <q-toolbar-title class="absolute-center">
          {{ title }}
        </q-toolbar-title>

        <q-btn
          v-if="!loggedIn"
          to="/auth"
          class="absolute-right q-pr-sm"
          icon="account_circle" 
          no-caps
          label="Login"
          flat/>
        <q-btn
          v-else
          to="/"
          class="absolute-right q-pr-sm"
          icon="account_circle" 
          @click="logout"
          no-caps
          dense
          flat>
           {{ account.user.firstName }}
           <br>
          Logout
          <br>
        </q-btn>
      </q-toolbar>
    </q-header>
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import EssentialLink from 'components/EssentialLink'
import mixinUserDetails from 'src/mixin_helpers/otherUserInfo.js'
export default {
  mixins:[mixinUserDetails],
  computed : {
      ...mapState({
          account: state => state.account
      }),
    loggedIn() {
      return this.$store.state.account.status.loggedIn
    },
    title(){
      let currentPath = this.$route.fullPath
      if (currentPath == '/') return "Quick Chat"
        else if (currentPath == '/auth') return "Login"
          else if (currentPath == '/chats') return "Chats"
            else if (currentPath.includes ('/messages')) return "Messages"
    }
  },
  methods : {
    ...mapActions('account',['logout'] ),
  }
}
</script>
<style lang="stylus">
  .q-toolbar
    .q-btn
      line-height: 1.2;
</style>
