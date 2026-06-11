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

<style scoped>
.admin-card {
  background: #ffffff;
  border-radius: 32px;
  padding: 2.5rem;
  box-shadow: 0 6px 25px rgba(175, 205, 240, 0.35);
  display: flex;
  flex-direction: column;
  height: 100%;
}

.card-icon-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid rgba(160, 195, 235, 0.3);
}

.section-icon {
  color: #1a73e8;
  background: #e8f0fe;
  padding: 0.6rem;
  border-radius: 50%;
  width: 42px;
  height: 42px;
}

.card-title {
  font-size: 1.25rem;
  font-weight: 500;
  color: #202124;
  margin: 0;
}

.form-group {
  margin-bottom: 1.5rem;
  display: flex;
  flex-direction: column;
}

.select-wrapper {
  position: relative;
  width: 100%;
}

.select-wrapper::after {
  content: "▼";
  font-size: 0.8rem;
  color: #5f6368;
  position: absolute;
  right: 1.2rem;
  top: 50%;
  transform: translateY(-50%);
  pointer-events: none;
}

.mt-auto {
  margin-top: auto; 
}

.btn-chrome-primary {
  width: 100%;
  background: #ffffff;
  color: #3c4043;
  border: 1px solid #c9defa;
  padding: 1rem;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 500;
  font-family: inherit;
  cursor: pointer;
  box-shadow: 0 4px 15px rgba(161, 194, 250, 0.4);
  transition: all 0.3s ease;
}

.btn-chrome-primary:hover {
  box-shadow: 0 6px 20px rgba(161, 194, 250, 0.6);
  color: #1a73e8;
  transform: translateY(-2px);
}
</style>