<script setup>
import { ref, onMounted } from 'vue'
import { jwtDecode } from 'jwt-decode'
import { RouterLink } from 'vue-router'
import Navbar from '@/components/navbar.vue'
import AdminRoleManager from '@/components/AdminRoleManager.vue'
import AdminProductForm from '@/components/AdminProductForm.vue'
import AdminCategoryManager from '@/components/AdminCategoryManager.vue'

const isAdmin = ref(false)
const activeTab = ref('product')

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

const selectTab = (tab) => {
  activeTab.value = tab
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

      <div class="admin-tabs">
        <button
          :class="['tab-button', { active: activeTab === 'product' }]"
          @click="selectTab('product')"
        >
          Dodaj nowy produkt
        </button>
        <button
          :class="['tab-button', { active: activeTab === 'users' }]"
          @click="selectTab('users')"
        >
          Zmień uprawnienia użytkownika
        </button>
        <button
          :class="['tab-button', { active: activeTab === 'categories' }]"
          @click="selectTab('categories')"
        >
          Zarządzaj kategoriami
        </button>
      </div>

      <div class="admin-tab-panel">
        <AdminProductForm v-if="activeTab === 'product'" />
        <AdminRoleManager v-else-if="activeTab === 'users'" />
        <AdminCategoryManager v-else-if="activeTab === 'categories'" />
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

<style scoped src="./AdminPageView.css"></style>