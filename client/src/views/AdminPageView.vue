<script setup>
import { ref, onMounted } from 'vue'
import { jwtDecode } from 'jwt-decode'
import { RouterLink } from 'vue-router'
import Navbar from '@/components/navbar.vue'
import { userService } from '@/services/UserService'
import { productService } from '@/services/ProductService'
import { productCategoryService } from '@/services/ProductCategoriesService'



const users = ref([
  ])

const fetchUsers= async ()=>{
  try{
    var request= await userService.getUsersList()
    users.value=request
  }catch(e){
    alert(e)
  }
}

const selectedUserId = ref('')
const selectedRoleId = ref('')
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

const handleRoleChange = async() => {
  if (!selectedUserId.value || !selectedRoleId.value) {
    alert('Wybierz użytkownika i nową rolę.')
    return
  }
  const payload = {
    userId: parseInt(selectedUserId.value),
    roleId: parseInt(selectedRoleId.value)
  }
  try{
    const request = await userService.changeUserRole(payload)
    alert(request.data.message)
    await fetchUsers()
  }catch (e){
    alert(e)
  }
}


const newProduct = ref({
  title: '',
  description: '',
  imageUrl: '',
  categories: [] 
})

const availableCategories = ref([]);

const fetchCategories = async () => {
  try {

    const response = productCategoryService.getAllCategories()
    availableCategories.value = response.data;
  } catch (error) {
    alert(error)
  }
};

const handleAddProduct = async() => {
  if (!newProduct.value.title) {
    alert('Tytuł produktu jest wymagany!')
    return
  }
  try{
  const request = productService.addProduct(newProduct)
  alert(`Dodano produkt: ${newProduct.value.title}`)
  // Czyszczenie formularza
  newProduct.value = {
    title: '',
    description: '',
    imageUrl: '',
    categories: []
  }
  }catch(e){alert(e)}
  
}

onMounted(async () => {
  checkUserRole()
  await fetchUsers()
  await fetchCategories()
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

          <form @submit.prevent="handleAddProduct,fetchCategories" class="product-form">
            
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

            <div class="form-group">
              <label for="categories" class="form-label">Kategorie (możesz wybrać kilka :))</label>
              <select id="categories" v-model="newProduct.categories" multiple class="chrome-input" style="height: auto; min-height: 100px;">
                <option v-for="category in availableCategories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
              <small style="color: #666; font-size: 0.8rem; margin-top: 4px; display: block;">
                Przytrzymaj klawisz <b>Ctrl</b> (Windows) lub <b>Cmd</b> (Mac), aby zaznaczyć wiele kategorii.
              </small>
            </div>

            <button type="submit" class="btn-chrome-action">
              Dodaj
            </button>
          </form>
        </section>

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