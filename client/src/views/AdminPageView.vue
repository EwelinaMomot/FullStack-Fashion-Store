<script setup>
import { ref } from 'vue'
import axios from 'axios'
import Navbar from '@/components/navbar.vue'


const users = ref([
  ])

const fetchUsers= async ()=>{
    const response =await axios.get("https://localhost:7154/api/users")
    users=response.data
    console.log("users from backend",users)
}
const selectedUserId = ref('')
const selectedRoleId = ref('')

const  handleRoleChange = async() => {
  if (!selectedUserId.value || !selectedRoleId.value) {
    alert('Wybierz użytkownika i nową rolę.')
    return
  }
  const changeUserRoleDTO=new{selectedUserId,selectedRoleId}
  try{
  const request = await axios.put("https://localhost:7154/api/users/${//TODO mojeid}/role",changeUserRoleDTO)
  
  console.log(request.data.message)
  }catch (e){
    console.log("błąd serwera przy aktualizacji roli:",e)
  }
}


const newProduct = ref({
  title: '',
  description: '',
  imageUrl: '',
  creatorUserId: ''
})

const handleAddProduct = async() => {
  if (!newProduct.value.title) {
    alert('Tytuł produktu jest wymagany!')
    return
  }
  // TODO newProduct.creatorUserId=
  try{
  const request = await axios.post("https://localhost:7154/api/products",newProduct)
  alert(`Dodano produkt: ${newProduct.value.title}`)
  // Czyszczenie formularza
  newProduct.value = {
    title: '',
    description: '',
    imageUrl: '',
    creatorUserId: ''
  }
  }catch(e){alert("Błąd serwera przy dodawaniu produktu")}
  
}
</script>

<template>
  <div class="admin-page-wrapper">
    <Navbar />
    <div class="admin-container">
      
      <header class="admin-header">
        <h1 class="page-title">Panel Administratora</h1>
        <p class="page-subtitle">Zarządzaj uprawnieniami i asortymentem sklepu.</p>
      </header>

      <div class="admin-grid">
        
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
                  {{ user.name }} (Obecnie: {{ user.role }})
                </option>
              </select>
            </div>
          </div>

          <div class="form-group">
            <label for="role-select" class="form-label">Zmień uprawnienia</label>
            <div class="select-wrapper">
              <select id="role-select" v-model="selectedRoleId" class="chrome-input">
                <option disabled value="">Wybierz rolę</option>
                <option value=2>Zwykły użytkownik (User)</option>
                <option value=1>Administrator (Admin)</option>
              </select>
            </div>
          </div>

          <button @click="handleRoleChange" class="btn-chrome-primary mt-auto">
            Zapisz uprawnienia
          </button>
        </section>


        <section class="admin-card">
          <div class="card-icon-header">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="section-icon"><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"></path><polyline points="3.27 6.96 12 12.01 20.73 6.96"></polyline><line x1="12" y1="22.08" x2="12" y2="12"></line></svg>
            <h2 class="card-title">Dodaj nowy produkt</h2>
          </div>

          <form @submit.prevent="handleAddProduct" class="product-form">
            
            <div class="form-group">
              <label for="title" class="form-label">Tytuł produktu</label>
              <input type="text" id="title" v-model="newProduct.title" placeholder="np. Biały T-shirt Basic" class="chrome-input" required />
            </div>

            <div class="form-group">
              <label for="imageUrl" class="form-label">URL zdjęcia</label>
              <input type="text" id="imageUrl" v-model="newProduct.imageUrl" placeholder="https://..." class="chrome-input" />
            </div>

            <div class="form-group">
              <label for="description" class="form-label">Opis </label>
              <textarea id="description" v-model="newProduct.description" rows="3" placeholder="Wprowadź opis produktu..." class="chrome-input chrome-textarea"></textarea>
            </div>

            <button type="submit" class="btn-chrome-action">
              Dodaj
            </button>
          </form>
        </section>

      </div>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600&display=swap');

.admin-page-wrapper {
  font-family: 'Outfit', sans-serif;
  background: radial-gradient(circle at 50% 20%, #dcecfa 0%, #edf4fc 40%, #ffffff 100%);
  min-height: 100vh;
  color: #3c4043;
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


.product-form {
  display: flex;
  flex-direction: column;
  flex-grow: 1;
}

.form-group {
  margin-bottom: 1.5rem;
  display: flex;
  flex-direction: column;
}

.form-label {
  font-size: 0.9rem;
  font-weight: 500;
  color: #5f6368;
  margin-bottom: 0.5rem;
  margin-left: 0.5rem;
}


.chrome-input {
  width: 100%;
  background: #f1f3f4; 
  border: 2px solid transparent;
  color: #202124;
  font-family: inherit;
  font-size: 1rem;
  padding: 0.9rem 1.2rem;
  border-radius: 24px; 
  transition: all 0.2s ease;
  appearance: none; 
  
}

.chrome-input:hover {
  background: #e8eaed;
}

.chrome-input:focus {
  outline: none;
  background: #ffffff;
  border-color: #1a73e8;
  box-shadow: 0 0 0 4px rgba(26, 115, 232, 0.15);
}

.chrome-textarea {
  border-radius: 16px; 
  resize: vertical;
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

.btn-chrome-action {
  width: 100%;
  background: #1a73e8; 
  color: #ffffff;
  border: none;
  padding: 1rem;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 500;
  font-family: inherit;
  cursor: pointer;
  box-shadow: 0 4px 15px rgba(26, 115, 232, 0.3);
  transition: all 0.3s ease;
  margin-top: 1rem;
}

.btn-chrome-action:hover {
  background: #1557b0;
  box-shadow: 0 6px 20px rgba(26, 115, 232, 0.4);
  transform: translateY(-2px);
}


</style>