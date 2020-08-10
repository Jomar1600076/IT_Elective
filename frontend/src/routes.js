import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)
  export const createRouter = new VueRouter({
    routes : [
    {
      path: '/',
      component: () => import('layouts/MainLayout.vue'),
      children: [
        { 
          path: '', 
          component: () => import('pages/Index.vue') ,
          name: 'home'
        },
        { 
          path: '/auth', 
          component: () => import('pages/Auth.vue'),
          name: 'auth'
        },
        { 
          path: '/chats', 
          component: () => import('./pages/Chats.vue'), 
          name: 'chats'
        },
        { 
          path: '/messages/:otherUserId', 
          component: () => import('pages/Messages.vue'),
          name: 'messages'  
        },
        { 
          path: '/users', 
          component: () => import('components/UserList.vue'),
          name: 'users'
        }
      ]
    }
],

    // Leave these as they are and change in quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    mode: process.env.VUE_ROUTER_MODE,
    base: process.env.VUE_ROUTER_BASE,
})