<script setup>
import { ref, onMounted } from 'vue'
import { jwtDecode } from 'jwt-decode'
import Navbar from '@/components/navbar.vue'
import AuthForm from '@/components/AuthForm.vue'
import UserProfile from '@/components/UserProfile.vue'

const isUserLogged = ref(false)
const username = ref('')

const checkUserSession = () => {
  const token = localStorage.getItem('token')
  if (token) {
    isUserLogged.value = true
    try {
      const decoded = jwtDecode(token)
      username.value = decoded.sub || ''
    } catch (error) {
      console.error('Błąd przy dekodowaniu tokenu:', error)
    }
  } else {
    isUserLogged.value = false
    username.value = ''
  }
}

const handleLoginSuccess = () => {
  checkUserSession()
}

const handleLogoutSuccess = () => {
  checkUserSession()
}

onMounted(() => {
  checkUserSession()
})
</script>

<template>
  <div class="auth-page-wrapper">
    <Navbar />
    
    <AuthForm 
      v-if="!isUserLogged" 
      @login-success="handleLoginSuccess" 
    />
    
    <UserProfile 
      v-else 
      :username="username" 
      @logout-success="handleLogoutSuccess" 
    />
  </div>
</template>

<style scoped src="./UserRegisterPageView.css"></style>