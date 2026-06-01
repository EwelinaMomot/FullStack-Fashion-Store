<script setup>
import { ref, onMounted } from 'vue'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import { productService } from '@/services/ProductService'
import { productCategoryService } from '@/services/ProductCategoriesService'
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'vue-router'


const router = useRouter()
const props = defineProps({
  productId: {
    type: String, 
    required: true
  }
})

const product = ref(null)
const isVisible = ref(false)
const isAdmin = ref(false)
const isEditModalOpen = ref(false)
const availableCategories = ref([])
const editFormData = ref({
  title: '',
  description: '',
  imageUrl: '',
  productCategoryIdList: []
})

const fetchProductData = async (id) => {
  try{
    const response = await  productService.getSingleProduct(id)
    product.value= response

  }catch(error){  alert("Nie udało się załadować produktu:", error)}
}

const checkUserRole= ()=>{
  const token = localStorage.getItem('token')
  if (token){
    try {
          const decodedPayload = jwtDecode(token);
          
          isAdmin.value = decodedPayload.role === 'Admin'; 
        } catch (error) {
          console.error("Nieprawidłowy token JWT", error);
        }
  }

}

const handleAddToCart =(product)=>{
  cartActions.addToCart(product)
}

const fetchCategories = async () => {
  try {
    const response = await productCategoryService.getAllCategories()
    availableCategories.value = response.categories
   
  } catch (error) {
    console.error('Błąd podczas pobierania kategorii:', error)
  }
}

const deleteProduct=async()=>{
  try{
    const response = await productService.deleteProduct(props.productId)
    alert("Pomyślnie usunięto produkt")
    router.push("/")
    
  }catch(e){alert(e)}
}

const openEditModal = async () => {
  await fetchCategories()
  editFormData.value = {
    title: product.value.title,
    description: product.value.description,
    imageUrl: product.value.imageUrl || '',
    productCategoryIdList: product.value.productCategoryIdList || []
  }
  isEditModalOpen.value = true

}

const saveEdit = async () => {
  if (!editFormData.value.title) {
    alert('Tytuł produktu jest wymagany!')
    return
  }
  try{
    const response = await productService.editProduct(props.productId, editFormData.value)
    alert("Pomyślnie zaktualizowano produkt")
    isEditModalOpen.value = false
    await fetchProductData(props.productId)
  }catch(e){
    alert(e)
  }
}

const closeEditModal = () => {
  isEditModalOpen.value = false
}


const showWithTimeout = () => {
  isVisible.value = true

  let timeout = null;
  timeout = setTimeout(()=>{isVisible.value=false},3000)
}

onMounted(async () => {
  try {
    await fetchProductData(props.productId)
  } catch (e) {
    console.log('błąd podczas pobierania danych z serwera', e)
  }
  checkUserRole()
})
</script>

<template>
  <div class="chrome-page-wrapper">
    <Navbar />
    <div v-if="product" class="product-container">
      
      <div class="image-section">
        <div class="chrome-card">
          <div class="metallic-border-overlay"></div>
          <img 
            :src="product.imageUrl || 'https://via.placeholder.com/600x800'" 
            :alt="product.title" 
            class="product-image"
          />
        </div>
      </div>

      <div class="info-section">
        <div class="categories-tags" v-if="product.productCategoryList && product.productCategoryList.length">
          <span 
            v-for="(category, index) in product.productCategoryList" 
            :key="index" 
            class="category-badge"
          >
            {{ category }}
          </span>
        </div>

        <h1 class="product-title">{{ product.title }}</h1>
        <div class="product-meta">
          <span class="meta-item">ID: {{ props.productId }}</span>
          <span v-if="product.creationDate" class="meta-item">
            Dodano: {{ new Date(product.creationDate).toLocaleDateString() }}
          </span>
        </div>

        <div class="divider-line"></div>

        <div class="description-container">
          <h3 class="section-subtitle">Opis</h3>
          <p class="product-description">{{ product.description }}</p>
        </div>

        <div class="divider-line"></div>

        <div class="actions-container">
          <button class="btn-chrome-primary" @click="handleAddToCart(product),showWithTimeout()">
            <span class="btn-text">Dodaj do koszyka</span>
            <div class="btn-glow"></div>
          </button>
          
        </div>
        <span v-if="isVisible">Dodano do koszyka</span>

        <div  v-if="isAdmin" class="admin-panel-box">
          <h4 class="admin-title">Panel administratora</h4>
          <p class="admin-desc">Jako administrator możesz trwale zaktualizować lub usunąć ten produkt ze stanów magazynowych.</p>
          <div class="admin-actions">
            <button class="btn-admin-edit" @click="openEditModal">Edytuj produkt</button>
            <button class="btn-admin-delete" @click="deleteProduct">Usuń produkt</button>
          </div>
        </div>

        <div v-if="isEditModalOpen" class="modal-overlay" @click.self="closeEditModal">
          <div class="modal-content">
            <div class="modal-header">
              <h3 class="modal-title">Edytuj produkt</h3>
              <button type="button" class="btn-close" @click="closeEditModal">&times;</button>
            </div>
            
            <form @submit.prevent="saveEdit" class="edit-form">
              <div class="form-group">
                <label for="edit-title" class="form-label">Tytuł produktu</label>
                <input 
                  type="text" 
                  id="edit-title" 
                  v-model="editFormData.title" 
                  placeholder="Nazwa produktu" 
                  class="chrome-input" 
                  required 
                />
              </div>

              <div class="form-group">
                <label for="edit-description" class="form-label">Opis</label>
                <textarea 
                  id="edit-description" 
                  v-model="editFormData.description" 
                  rows="4" 
                  placeholder="Opis produktu..." 
                  class="chrome-input chrome-textarea"
                ></textarea>
              </div>

              <div class="form-group">
                <label for="edit-imageUrl" class="form-label">URL zdjęcia</label>
                <input 
                  type="text" 
                  id="edit-imageUrl" 
                  v-model="editFormData.imageUrl" 
                  placeholder="https://..." 
                  class="chrome-input" 
                />
              </div>

              <div class="form-group">
                <label for="edit-categories" class="form-label">Kategorie</label>
                <select 
                  id="edit-categories" 
                  v-model="editFormData.productCategoryIdList" 
                  multiple 
                  class="chrome-input" 
                  style="height: auto; min-height: 100px;"
                >
                  <option v-for="category in availableCategories" :key="category.id" :value="category.id">
                    {{ category.name }}
                  </option>
                </select>
                <small style="color: #666; font-size: 0.8rem; margin-top: 4px; display: block;">
                  Przytrzymaj klawisz <b>Ctrl</b> (Windows) lub <b>Cmd</b> (Mac), aby zaznaczyć wiele kategorii.
                </small>
              </div>

              <div class="modal-actions">
                <button type="button" class="btn-modal-cancel" @click="closeEditModal">Anuluj</button>
                <button type="submit" class="btn-modal-save">Zapisz zmiany</button>
              </div>
            </form>
          </div>
        </div>

      </div>
    </div>

    <div v-else class="loading-state">
      <div class="chrome-spinner"></div>
      <p>Pobieranie danych...</p>
    </div>
  </div>
</template>

<style scoped>
.chrome-page-wrapper {
  background: radial-gradient(circle at 50% 20%, var(--color-gradient-start) 0%, var(--color-gradient-mid) 40%, var(--color-gradient-end) 100%);
  min-height: 100vh;
  color: var(--color-text);
  padding: 0 1.5rem 4rem;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
}

.product-container {
  max-width: 1100px;
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
  margin-top: 2rem;
}


.chrome-card {
  background: #ffffff;
  border-radius: 32px;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(175, 205, 240, 0.35);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;

  background: #f4f8fd; 
  transition: transform 0.3s ease;
}

.product-image {
  width: 100%;
  height: auto;
  border-radius: 16px;
  object-fit: contain;
  max-height: 600px;
}


.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.categories-tags {
  margin-bottom: 1rem;
}


.category-badge {
  background: #e8f0fe;
  color: #1a73e8;
  padding: 0.4rem 1rem;
  border-radius: 24px;
  font-size: 0.85rem;
  font-weight: 500;
  display: inline-block;
  margin-right: 0.5rem;
  margin-bottom: 0.5rem;
}

.product-title {
  font-size: 2.5rem;
  font-weight: 500;
  color: #202124;
  margin: 0 0 0.5rem 0;
  line-height: 1.2;
}

.product-meta {
  display: flex;
  gap: 1.5rem;
  color: #5f6368;
  font-size: 0.95rem;
  font-weight: 300;
  margin-bottom: 1.5rem;
}

.divider-line {
  height: 1px;
  background: rgba(160, 195, 235, 0.3);
  width: 100%;
  margin: 1.5rem 0;
}

.section-subtitle {
  font-size: 1.25rem;
  font-weight: 500;
  color: #202124;
  margin-bottom: 0.75rem;
}

.product-description {
  font-weight: 300;
  color: #5f6368;
  line-height: 1.6;
  font-size: 1rem;
  margin: 0;
}

.actions-container {
  margin-top: 1rem;
}


.admin-panel-box {
  background: #ffffff;
  border-radius: 24px;
  padding: 1.5rem;
  box-shadow: 0 6px 20px rgba(175, 205, 240, 0.25);
  margin-top: 3rem;
  border: 1px solid rgba(160, 195, 235, 0.15);
}

.admin-title {
  margin: 0 0 0.5rem 0;
  font-weight: 500;
  color: #202124;
  font-size: 1.1rem;
}

.admin-desc {
  font-weight: 300;
  color: #5f6368;
  font-size: 0.9rem;
  margin-bottom: 1.25rem;
}

.admin-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: #ffffff;
  border-radius: 32px;
  padding: 2rem;
  max-width: 600px;
  width: 100%;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid rgba(160, 195, 235, 0.3);
}

.modal-title {
  font-size: 1.5rem;
  font-weight: 500;
  color: #202124;
  margin: 0;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  color: #5f6368;
  cursor: pointer;
  padding: 0;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s ease;
}

.btn-close:hover {
  color: #202124;
}

.edit-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-label {
  font-size: 0.9rem;
  font-weight: 500;
  color: var(--color-text-light);
  margin-bottom: 0.5rem;
  margin-left: 0.5rem;
}

.chrome-textarea {
  resize: vertical;
  font-family: inherit;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid rgba(160, 195, 235, 0.3);
}

.btn-modal-cancel, .btn-modal-save {
  padding: 0.8rem 1.5rem;
  border-radius: 24px;
  font-family: inherit;
  font-size: 0.95rem;
  font-weight: 500;
  cursor: pointer;
  border: none;
  transition: all 0.2s ease;
}

.btn-modal-cancel {
  background: #e8eaed;
  color: #3c4043;
}

.btn-modal-cancel:hover {
  background: #dadce0;
}

.btn-modal-save {
  background: #1a73e8;
  color: #ffffff;
  box-shadow: 0 4px 15px rgba(26, 115, 232, 0.3);
}

.btn-modal-save:hover {
  background: #1557b0;
  box-shadow: 0 6px 20px rgba(26, 115, 232, 0.4);
  transform: translateY(-2px);
}

.loading-state {
  text-align: center;
  color: #5f6368;
  font-weight: 300;
  margin-top: 4rem;
  font-size: 1.1rem;
}


@media (max-width: 900px) {
  .product-container {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .product-title {
    font-size: 2rem;
  }
}
</style>