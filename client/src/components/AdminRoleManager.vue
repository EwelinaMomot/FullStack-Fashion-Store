<script setup>
import { ref, onMounted } from 'vue'
import { userService } from '@/services/UserService'

const users = ref([])
const selectedUserId = ref('')
const selectedRoleId = ref('')

const fetchUsers = async () => {
  try {
    var request = await userService.getUsersList()
    users.value = request
  } catch(e) {
    alert(e)
  }
}

const handleRoleChange = async () => {
  if (!selectedUserId.value || !selectedRoleId.value) {
    alert('Wybierz użytkownika i nową rolę.')
    return
  }
  const payload = {
    userId: parseInt(selectedUserId.value),
    roleId: parseInt(selectedRoleId.value)
  }
  try {
    const request = await userService.changeUserRole(payload)
    alert("Pomyślnie zmieniono rolę!")
    await fetchUsers()
  } catch (e) {
    alert(e)
  }
}

onMounted(async () => {
  await fetchUsers()
})
</script>

<template>
  <section class="admin-card">
    <div class="card-icon-header">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="section-icon"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
      <h2 class="card-title">Uprawnienia Kont</h2>
    </div>
    
    <div class="form-group">
      <label for="user-select" class="form-label">Wybierz użytkownika</label>
      <div class="select-wrapper">
        <select id="user-select" v-model="selectedUserId" class="chrome-input">
          <option disabled value="">Wybierz z listy</option>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.username }} (Obecnie: {{ user.roleName }})
          </option>
        </select>
      </div>
    </div>

    <div class="form-group">
      <label for="role-select" class="form-label">Zmień uprawnienia</label>
      <div class="select-wrapper">
        <select id="role-select" v-model="selectedRoleId" class="chrome-input">
          <option disabled value="">Wybierz rolę</option>
          <option value="2">Zwykły użytkownik (User)</option>
          <option value="1">Administrator (Admin)</option>
        </select>
      </div>
    </div>

    <button @click="handleRoleChange" class="btn-chrome-primary mt-auto">
      Zapisz uprawnienia
    </button>
  </section>
</template>

<style scoped src="./AdminRoleManager.css"></style>