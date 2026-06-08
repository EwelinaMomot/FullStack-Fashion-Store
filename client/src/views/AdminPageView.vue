<script setup>
import { ref, onMounted } from 'vue'
import { jwtDecode } from 'jwt-decode'
import { RouterLink } from 'vue-router'
import Navbar from '@/components/navbar.vue'
import AdminRoleManager from '@/components/AdminRoleManager.vue'
import AdminProductForm from '@/components/AdminProductForm.vue'

const isAdmin = ref(false)

const checkUserRole = () => {
  const token = localStorage.getItem('token')
  if (token){
    try {
      const decodedPayload = jwtDecode(token)
      isAdmin.value = decodedPayload.role === 'Admin'
    } catch (error) {
      console.error("Nieprawidłowy token JWT", error)
    }
  }
}

onMounted(() => {
  checkUserRole()
})
</script>

<template>
  <div class="admin-page-wrapper">
    <Navbar />
    <div v-if="isAdmin" class="admin-container">
      
      <header class="admin-header">
        <h1 class="page-title">Panel Administratora</h1>
        <p class="page-subtitle">Zarządzaj uprawnieniami i asortymentem sklepu.</p>
      </header>

      <div class="admin-grid">
        <AdminRoleManager />
        <AdminProductForm />
      </div>
    </div>

    <div v-else class="access-denied-container">
      <div class="access-denied-card">
        <div class="denied-icon-wrapper">
          <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"></circle>
            <line x1="12" y1="8" x2="12" y2="12"></line>
            <line x1="12" y1="16" x2="12.01" y2="16"></line>
          </svg>
        </div>
        <h2 class="denied-title">Dostęp zabroniony</h2>
        <p class="denied-message">Ten panel jest dostępny tylko dla administratorów. Nie masz uprawnień, aby przeglądać tę zawartość.</p>
        <RouterLink to="/" class="btn-back-home">
          Wróć do strony głównej
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>
.admin-page-wrapper {
  background: radial-gradient(circle at 50% 20%, var(--color-gradient-start) 0%, var(--color-gradient-mid) 40%, var(--color-gradient-end) 100%);
  min-height: 100vh;
  color: var(--color-text);
  padding: 4rem 1.5rem;
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
  align-items: center;
}

.admin-container {
  max-width: 1000px;
  width: 100%;
}

.admin-header {
  text-align: center;
  margin-bottom: 4rem;
}

.page-title {
  font-size: 2.25rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 0.5rem 0;
}

.page-subtitle {
  font-weight: 300;
  color: #5f6368;
  font-size: 1.1rem;
  margin: 0;
}

.admin-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2.5rem;
  align-items: start;
}

.access-denied-container {
  width: 100%;
  max-width: 600px;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 60vh;
}

.access-denied-card {
  background: #ffffff;
  border-radius: 32px;
  padding: 3rem 2.5rem;
  box-shadow: 0 10px 30px rgba(175, 205, 240, 0.4);
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.denied-icon-wrapper {
  color: #d93025;
  background: #fce8e6;
  width: 80px;
  height: 80px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1.5rem;
}

.denied-title {
  font-size: 1.75rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 1rem 0;
}

.denied-message {
  font-size: 1rem;
  font-weight: 300;
  color: #5f6368;
  margin: 0 0 2rem 0;
  line-height: 1.6;
}

.btn-back-home {
  display: inline-block;
  background: #1a73e8;
  color: #ffffff;
  text-decoration: none;
  padding: 0.9rem 2rem;
  border-radius: 24px;
  font-weight: 500;
  box-shadow: 0 4px 15px rgba(26, 115, 232, 0.3);
  transition: all 0.3s ease;
}

.btn-back-home:hover {
  background: #1557b0;
  box-shadow: 0 6px 20px rgba(26, 115, 232, 0.4);
  transform: translateY(-2px);
}
</style>