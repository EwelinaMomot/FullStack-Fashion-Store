<script setup>
import { ref, onMounted } from 'vue'
import { productService } from '@/services/ProductService'
import { productCategoryService } from '@/services/ProductCategoriesService'
import Modal from '@/components/Modal.vue'

const newProduct = ref({
  title: '',
  description: '',
  imageUrl: '',
  productCategoryIdList: []
})

const availableCategories = ref([])
const isAddCategoryModalOpen = ref(false)
const newCategoryName = ref('')

const fetchCategories = async () => {
  try {
    const response = await productCategoryService.getAllCategories()
    availableCategories.value = response.categories
  } catch (error) {
    alert(error)
  }
}

const handleAddProduct = async() => {
  if (!newProduct.value.title) {
    alert('Tytuł produktu jest wymagany!')
    return
  }
  try {
    await productService.addProduct(newProduct.value)
    alert(`Dodano produkt: ${newProduct.value.title}`)
    newProduct.value = {
      title: '',
      description: '',
      imageUrl: '',
      productCategoryIdList: []
    }
  } catch(e) {
    alert(e)
  }
}

const openAddCategoryModal = () => {
  newCategoryName.value = ''
  isAddCategoryModalOpen.value = true
}

const closeAddCategoryModal = () => {
  isAddCategoryModalOpen.value = false
}

const saveNewCategory = async () => {
  if (!newCategoryName.value.trim()) {
    alert('Nazwa kategorii jest wymagana!')
    return
  }

  try {
    await productCategoryService.addCategory({ name: newCategoryName.value.trim() })
    alert(`Dodano nową kategorię: ${newCategoryName.value.trim()}`)
    await fetchCategories()
    closeAddCategoryModal()
  } catch (error) {
    alert(error)
  }
}

onMounted(async () => {
  await fetchCategories()
})
</script>

<template>
  <section class="admin-card">
    <div class="card-icon-header">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="section-icon"><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"></path><polyline points="3.27 6.96 12 12.01 20.73 6.96"></polyline><line x1="12" y1="22.08" x2="12" y2="12"></line></svg>
      <div>
        <h2 class="card-title">Dodaj nowy produkt</h2>
        <button type="button" class="btn-add-category" @click="openAddCategoryModal">
          Dodaj nową kategorię
        </button>
      </div>
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

      <div class="form-group">
        <label for="categories" class="form-label">Kategorie (możesz wybrać kilka :))</label>
        <select id="categories" v-model="newProduct.productCategoryIdList" multiple class="chrome-input" style="height: auto; min-height: 100px;">
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

    <Modal
      :is-open="isAddCategoryModalOpen"
      title="Dodaj nową kategorię"
      :on-close="closeAddCategoryModal"
      submitButtonText="Dodaj kategorię"
      cancelButtonText="Anuluj"
      @submit="saveNewCategory"
    >
      <template #body>
        <div class="form-group">
          <label for="new-category-name" class="form-label">Nazwa kategorii</label>
          <input
            type="text"
            id="new-category-name"
            v-model="newCategoryName"
            placeholder="np. Akcesoria"
            class="chrome-input"
            required
          />
        </div>
      </template>
    </Modal>
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

.btn-add-category {
  margin-top: 0.75rem;
  display: inline-flex;
  background: #ffffff;
  border: 1px solid #1a73e8;
  color: #1a73e8;
  padding: 0.7rem 1rem;
  border-radius: 9999px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-add-category:hover {
  background: #e8f0fe;
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