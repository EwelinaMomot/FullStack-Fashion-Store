<script setup>
const props = defineProps({
  username: {
    type: String,
    required: true
  }
})

const emit = defineEmits(['logout-success'])

const logout = () => {
  localStorage.removeItem('token')
  emit('logout-success')
  alert('Wylogowano pomyślnie!')
}
</script>

<template>
  <div class="logged-in-card">
    <div class="logged-in-content">
      <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="user-icon">
        <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
        <circle cx="12" cy="7" r="4"></circle>
      </svg>
      <h2 class="logged-in-title">Zalogowano na konto</h2>
      <p class="username-display">{{ username }}</p>
      <button @click="logout" class="btn-logout">Wyloguj się</button>
    </div>
  </div>
</template>

<style scoped src="./UserProfile.css"></style>