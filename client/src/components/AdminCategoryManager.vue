<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import Modal from '@/components/Modal.vue'
import { productCategoryService } from '@/services/ProductCategoriesService'

const categories = ref([])
const isAddModalOpen = ref(false)
const isDeleteModalOpen = ref(false)
const isEditModalOpen = ref(false)
const newCategoryName = ref('')
const selectedDeleteCategoryId = ref('')
const selectedEditCategoryId = ref('')
const editCategoryName = ref('')

const fetchCategories = async () => {
  try {
    const response = await productCategoryService.getAllCategories()
    categories.value = response.categories || response
  } catch (error) {
    alert(error)
  }
}

const openAddCategoryModal = () => {
  newCategoryName.value = ''
  isAddModalOpen.value = true
}

const closeAddCategoryModal = () => {
  isAddModalOpen.value = false
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

const openDeleteCategoryModal = () => {
  selectedDeleteCategoryId.value = categories.value.length ? categories.value[0].id : ''
  isDeleteModalOpen.value = true
}

const closeDeleteCategoryModal = () => {
  isDeleteModalOpen.value = false
}

const deleteCategory = async () => {
  if (!selectedDeleteCategoryId.value) {
    alert('Wybierz kategorię do usunięcia.')
    return
  }

  try {
    await productCategoryService.deleteCategory(selectedDeleteCategoryId.value)
    alert('Kategoria została usunięta.')
    await fetchCategories()
    closeDeleteCategoryModal()
  } catch (error) {
    alert(error)
  }
}

const openEditCategoryModal = () => {
  selectedEditCategoryId.value = categories.value.length ? categories.value[0].id : ''
  updateEditName()
  isEditModalOpen.value = true
}

const closeEditCategoryModal = () => {
  isEditModalOpen.value = false
}

const updateEditName = () => {
  const category = categories.value.find(c => c.id === selectedEditCategoryId.value)
  editCategoryName.value = category ? category.name : ''
}

watch(selectedEditCategoryId, () => {
  updateEditName()
})

const saveEditedCategory = async () => {
  if (!selectedEditCategoryId.value) {
    alert('Wybierz kategorię do edycji.')
    return
  }

  if (!editCategoryName.value.trim()) {
    alert('Nazwa kategorii jest wymagana!')
    return
  }

  try {
    await productCategoryService.editCategory(selectedEditCategoryId.value, { name: editCategoryName.value.trim() })
    alert('Kategoria została zaktualizowana.')
    await fetchCategories()
    closeEditCategoryModal()
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
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="section-icon"><path d="M4 6h16M4 12h16M4 18h16"></path></svg>
      <h2 class="card-title">Zarządzaj kategoriami</h2>
    </div>

    <div class="category-action-row">
      <button type="button" class="btn-chrome-action" @click="openAddCategoryModal">Dodaj nową kategorię</button>
      <button type="button" class="btn-chrome-secondary" @click="openDeleteCategoryModal">Usuń kategorię</button>
      <button type="button" class="btn-chrome-secondary" @click="openEditCategoryModal">Edytuj kategorię</button>
    </div>

    <div class="category-list-card">
      <h3 class="section-subtitle">Dostępne kategorie</h3>
      <ul class="category-list">
        <li v-for="category in categories" :key="category.id">{{ category.name }}</li>
      </ul>
    </div>

    <Modal
      :is-open="isAddModalOpen"
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

    <Modal
      :is-open="isDeleteModalOpen"
      title="Usuń kategorię"
      :on-close="closeDeleteCategoryModal"
      submitButtonText="Usuń"
      cancelButtonText="Anuluj"
      @submit="deleteCategory"
    >
      <template #body>
        <div class="form-group">
          <label for="delete-category-select" class="form-label">Wybierz kategorię do usunięcia</label>
          <select id="delete-category-select" v-model="selectedDeleteCategoryId" class="chrome-input">
            <option disabled value="">Wybierz kategorię</option>
            <option v-for="category in categories" :key="category.id" :value="category.id">{{ category.name }}</option>
          </select>
        </div>
      </template>
    </Modal>

    <Modal
      :is-open="isEditModalOpen"
      title="Edytuj kategorię"
      :on-close="closeEditCategoryModal"
      submitButtonText="Zapisz"
      cancelButtonText="Anuluj"
      @submit="saveEditedCategory"
    >
      <template #body>
        <div class="form-group">
          <label for="edit-category-select" class="form-label">Wybierz kategorię</label>
          <select id="edit-category-select" v-model="selectedEditCategoryId" class="chrome-input">
            <option disabled value="">Wybierz kategorię</option>
            <option v-for="category in categories" :key="category.id" :value="category.id">{{ category.name }}</option>
          </select>
        </div>
        <div class="form-group">
          <label for="edit-category-name" class="form-label">Nowa nazwa kategorii</label>
          <input
            type="text"
            id="edit-category-name"
            v-model="editCategoryName"
            placeholder="Nowa nazwa kategorii"
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
  margin-bottom: 1.5rem;
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

.category-action-row {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1.75rem;
}

.btn-chrome-secondary {
  background: #ffffff;
  border: 1px solid #d2e3fc;
  color: #1a73e8;
  padding: 0.9rem 1.5rem;
  border-radius: 9999px;
  cursor: pointer;
  transition: all 0.25s ease;
}

.btn-chrome-secondary:hover {
  background: #e8f0fe;
}

.section-subtitle {
  font-size: 1rem;
  color: #5f6368;
  margin-bottom: 1rem;
  font-weight: 400;
}

.category-list-card {
  background: #f7fbff;
  border-radius: 24px;
  padding: 1.75rem;
  border: 1px solid rgba(160, 195, 235, 0.3);
}

.category-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: grid;
  gap: 0.85rem;
}

.category-list li {
  padding: 0.85rem 1rem;
  background: #ffffff;
  border-radius: 16px;
  border: 1px solid rgba(160, 195, 235, 0.25);
}

.form-group {
  margin-bottom: 1.5rem;
  display: flex;
  flex-direction: column;
}
</style>
