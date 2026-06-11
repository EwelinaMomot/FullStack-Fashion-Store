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

<style scoped src="./ProductAdminPanel.css"></style>