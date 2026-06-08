<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import Modal from '@/components/Modal.vue'
import { productService } from '@/services/ProductService'
import { productCategoryService } from '@/services/ProductCategoriesService'

const props = defineProps({
  productId: {
    type: String, 
    required: true
  },
  product: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['productUpdated'])
const router = useRouter()

const isEditModalOpen = ref(false)
const availableCategories = ref([])
const editFormData = ref({
  title: '',
  description: '',
  imageUrl: '',
  productCategoryIdList: []
})

const fetchCategories = async () => {
  try {
    const response = await productCategoryService.getAllCategories()
    availableCategories.value = response.categories
  } catch (error) {
    console.error('Błąd podczas pobierania kategorii:', error)
  }
}

const deleteProduct = async () => {
  try {
    await productService.deleteProduct(props.productId)
    alert("Pomyślnie usunięto produkt")
    router.push("/")
  } catch(e) {
    alert(e)
  }
}

const openEditModal = async () => {
  await fetchCategories()
  editFormData.value = {
    title: props.product.title,
    description: props.product.description,
    imageUrl: props.product.imageUrl || '',
    productCategoryIdList: props.product.productCategoryIdList || []
  }
  isEditModalOpen.value = true
}

const saveEdit = async () => {
  if (!editFormData.value.title) {
    alert('Tytuł produktu jest wymagany!')
    return
  }
  try {
    await productService.editProduct(props.productId, editFormData.value)
    alert("Pomyślnie zaktualizowano produkt")
    isEditModalOpen.value = false
    emit('productUpdated')
  } catch(e) {
    alert(e)
  }
}

const closeEditModal = () => {
  isEditModalOpen.value = false
}
</script>

<template>
  <div class="admin-panel-box">
    <h4 class="admin-title">Panel administratora</h4>
    <p class="admin-desc">Jako administrator możesz trwale zaktualizować lub usunąć ten produkt ze stanów magazynowych.</p>
    <div class="admin-actions">
      <button class="btn-admin-edit" @click="openEditModal">Edytuj produkt</button>
      <button class="btn-admin-delete" @click="deleteProduct">Usuń produkt</button>
    </div>

    <Modal 
      :is-open="isEditModalOpen" 
      title="Edytuj produkt"
      :on-close="closeEditModal"
      @submit="saveEdit"
    >
      <template #body>
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
        </form>
      </template>
    </Modal>
  </div>
</template>

<style scoped>
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

.btn-admin-edit {
  background: #ffffff;
  border: 1px solid #1a73e8;
  color: #1a73e8;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-admin-edit:hover {
  background: #e8f0fe;
}

.btn-admin-delete {
  background: #ffffff;
  border: 1px solid #d93025;
  color: #d93025;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-admin-delete:hover {
  background: #fce8e6;
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
</style>