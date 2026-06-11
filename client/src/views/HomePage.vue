<script setup>
import { ref, onMounted, computed } from 'vue'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import ProductCard from '@/components/ProductCard.vue'
import { productService } from '@/services/ProductService'
import { productCategoryService } from '@/services/ProductCategoriesService'

const products = ref([])
const categories = ref([])
const selectedCategoryId = ref('')
const searchTerm = ref('')
const pagination = ref({ currentPage: 1, totalPages: 0, totalProductsNumber: 0 })
const pageSize = 3
const pages = computed(() => Array.from({ length: pagination.value.totalPages }, (_, index) => index + 1))

const fetchCategories = async () => {
  try {
    const response = await productCategoryService.getAllCategories()
    categories.value = response.categories ?? response
  } catch (error) {
    alert(error)
  }
}

const fetchProducts = async (page = 1) => {
  try {
    const categoryId = selectedCategoryId.value ? Number(selectedCategoryId.value) : undefined
    const response = await productService.getProductList(page, pageSize, categoryId, searchTerm.value)
    products.value = response.products
    pagination.value.currentPage = response.currentPage
    pagination.value.totalPages = response.totalPages
    pagination.value.totalProductsNumber = response.totalProductsNumber
  } catch (error) {
    alert("Nie udało się załadować produktów:", error)
  }
}

onMounted(() => {
  fetchCategories()
  fetchProducts()
})
</script>

  <template>
    <div class="home-root">
      <Navbar />
      <header class="hero">
        <div class="hero-inner">
          <h2 class="brand">E/M Studio Fashion Store</h2>
          <p class="tagline">Zapraszamy</p>
          <a class="cta" href="#products">Zobacz moją kolekcję</a>
        </div>
      </header>

      <main class="container">
        <section id="products" class="products-section">
          <h3 class="section-title">Wybrane produkty</h3>

          <div class="filter-row">
            <div class="filter-search">
              <label for="search-input" class="filter-label">Szukaj produktu</label>
              <input
                id="search-input"
                type="text"
                v-model="searchTerm"
                @keyup.enter="fetchProducts(1)"
                placeholder="Szukaj..."
                class="chrome-input filter-input"
              />
            </div>

            <div class="filter-category">
              <label for="category-filter" class="filter-label">Filtr kategorii</label>
              <select
                id="category-filter"
                v-model="selectedCategoryId"
                @change="fetchProducts(1)"
                class="chrome-input filter-select"
              >
                <option value="">Wszystkie kategorie</option>
                <option
                  v-for="category in categories"
                  :key="category.id"
                  :value="category.id"
                >
                  {{ category.name }}
                </option>
              </select>
            </div>
          </div>

          <p v-if="products.length === 0" class="loading">Ładowanie produktów lub brak danych...</p>

         <div v-else class="grid">
            <ProductCard 
              v-for="product in products" 
              :key="product.id" 
              :product="product"
              :on-add-to-cart="cartActions.addToCart"
            />
          </div>

          <div v-if="pagination.totalPages > 1" class="pagination">
            <button
              class="page-button"
              :disabled="pagination.currentPage === 1"
              @click="fetchProducts(pagination.currentPage - 1)"
            >
              Poprzednia
            </button>

            <button
              v-for="page in pages"
              :key="page"
              :class="['page-button', { active: page === pagination.currentPage }]"
              @click="fetchProducts(page)"
            >
              {{ page }}
            </button>

            <button
              class="page-button"
              :disabled="pagination.currentPage === pagination.totalPages"
              @click="fetchProducts(pagination.currentPage + 1)"
            >
              Następna
            </button>
          </div>

          <p class="pagination-summary">Strona {{ pagination.currentPage }} z {{ pagination.totalPages }} </p>
        </section>
      </main>
    </div>
  </template>

  <style scoped src="./HomePage.css"></style>
