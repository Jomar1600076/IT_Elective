 
 export default {
   computed: {
      chatPairInfo(){
        return this.$store.state.users.allUsers.allUsers[this.$route.params.otherUserId]
      }
   },
 };
