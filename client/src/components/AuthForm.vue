<script setup>
import { ref } from 'vue'
import { AuthService } from '@/services/AuthService'

const emit = defineEmits(['login-success'])

const isLoginMode = ref(true)
const formData = ref({
  username: '',
  password: ''
})

const toggleMode = () => {
  isLoginMode.value = !isLoginMode.value
  formData.value = { username: '', password: '' }
}

const handleSubmit = async () => {
  if (isLoginMode.value) {
    try {
      var response = await AuthService.login(formData.value)
      var token = response
      localStorage.setItem('token', token)
      emit('login-success')
      alert("Pomyślnie zalogowano!")
      formData.value = { username: '', password: '' }
    } catch(error) {
      alert(error)
    }
  } else {
    try {
      await AuthService.register(formData.value)
      alert("Pomyślnie zarejestrowano! Zaloguj się na swoje nowe konto :) ")
      formData.value = { username: '', password: '' }
    } catch(error) {
      alert(error)
    }
  }
}
</script>

<template>
  <div class="auth-card">
    <header class="auth-header">
      <div class="icon-wrapper">
        <svg v-if="isLoginMode" xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"></path><polyline points="10 17 15 12 10 7"></polyline><line x1="15" y1="12" x2="3" y2="12"></line></svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="8.5" cy="7" r="4"></circle><line x1="20" y1="8" x2="20" y2="14"></line><line x1="23" y1="11" x2="17" y2="11"></line></svg>
      </div>
      <h1 class="auth-title">{{ isLoginMode ? 'Zaloguj się' : 'Utwórz konto' }}</h1>
      <p class="auth-subtitle">
        {{ isLoginMode ? 'Witaj ponownie! Zaloguj się, aby kontynuować.' : 'Dołącz i zyskaj dostęp do nowej kolekcji.' }}
      </p>
    </header>

    <form @submit.prevent="handleSubmit" class="auth-form">
      <div class="form-group">
        <label for="username" class="form-label">Nazwa użytkownika</label>
        <input 
          type="text" 
          id="username" 
          v-model="formData.username" 
          placeholder="Twoja nazwa :)" 
          class="chrome-input" 
          required 
        />
      </div>

      <div class="form-group">
        <label for="password" class="form-label">Hasło</label>
        <input 
          type="password" 
          id="password" 
          v-model="formData.password" 
          placeholder="••••••••" 
          class="chrome-input" 
          required 
        />
      </div>

      <button type="submit" class="btn-chrome-action">
        {{ isLoginMode ? 'Zaloguj się' : 'Zarejestruj się' }}
      </button>
    </form>

    <div class="auth-footer">
      <p v-if="isLoginMode" class="toggle-text">
        Nie masz jeszcze konta? 
        <button @click="toggleMode" type="button" class="btn-toggle-mode">Zarejestruj się</button>
      </p>
      <p v-else class="toggle-text">
        Masz już konto? 
        <button @click="toggleMode" type="button" class="btn-toggle-mode">Zaloguj się</button>
      </p>
    </div>
  </div>
</template>

<style scoped>
.auth-card {
  background: #ffffff;
  border-radius: 32px;
  width: 100%;
  max-width: 440px;
  padding: 3rem 2.5rem;
  box-shadow: 0 10px 30px rgba(175, 205, 240, 0.4);
  display: flex;
  flex-direction: column;
}

.auth-header {
  text-align: center;
  margin-bottom: 2rem;
}

.icon-wrapper {
  color: #1a73e8; 
  background: #e8f0fe; 
  width: 64px;
  height: 64px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1.5rem auto;
}

.auth-title {
  font-size: 2rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 0.5rem 0;
}

.auth-subtitle {
  font-size: 1rem;
  font-weight: 300;
  color: #5f6368;
  margin: 0;
  line-height: 1.5;
}

.auth-form {
  display: flex;
  flex-direction: column;
}

.form-group {
  margin-bottom: 1.5rem;
  display: flex;
  flex-direction: column;
}

.auth-footer {
  margin-top: 2rem;
  text-align: center;
  border-top: 1px solid rgba(160, 195, 235, 0.3);
  padding-top: 1.5rem;
}

.toggle-text {
  font-size: 0.95rem;
  color: #5f6368;
  margin: 0;
}

.btn-toggle-mode {
  background: none;
  border: none;
  color: #1a73e8;
  font-weight: 500;
  font-family: inherit;
  font-size: 0.95rem;
  cursor: pointer;
  padding: 0;
  margin-left: 0.25rem;
  text-decoration: underline;
  text-underline-offset: 4px;
  text-decoration-color: transparent;
  transition: text-decoration-color 0.2s ease;
}

.btn-toggle-mode:hover {
  text-decoration-color: #1a73e8;
}
</style>