<script setup>
import { ref, onMounted, computed } from 'vue'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import ProductCard from '@/components/ProductCard.vue'
import { productService } from '@/services/ProductService'

const products = ref([])
const pagination = ref({ currentPage: 1, totalPages: 0, totalProductsNumber: 0 })
const pageSize = 3
const pages = computed(() => Array.from({ length: pagination.value.totalPages }, (_, index) => index + 1))

const fetchProducts = async (page = 1) => {
  try {
    const response = await productService.getProductList(page, pageSize)
    products.value = response.products
    pagination.value.currentPage = response.currentPage
    pagination.value.totalPages = response.totalPages
    pagination.value.totalProductsNumber = response.totalProductsNumber
  } catch (error) {
    alert("Nie udało się załadować produktów:", error)
  }
}


onMounted(() => {
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


  <style scoped>
.home-root {
  background: radial-gradient(circle at 50% 20%, var(--color-gradient-start) 0%, var(--color-gradient-mid) 40%, var(--color-gradient-end) 100%);
  min-height: 100vh;
  color: var(--color-text);
  padding-bottom: 4rem;
}

.hero {
  text-align: center;
  padding: 8rem 1rem 4rem;
}

.brand {
  font-size: 2.5rem;
  font-weight: 300; 
  color: #202124;
  margin-bottom: 0.5rem;
}

.tagline {
  font-size: 1.25rem;
  font-weight: 300;
  color: #5f6368;
  margin-bottom: 2.5rem;
}

.cta {
  display: inline-block;
  text-decoration: none;
  background: #ffffff;
  color: #3c4043;
  padding: 1rem 2.5rem;
  border-radius: 40px; 
  font-size: 1rem;
  font-weight: 500;

  box-shadow: 0 8px 24px rgba(160, 195, 235, 0.4); 
  transition: all 0.3s ease;
}

.cta:hover {
  box-shadow: 0 12px 32px rgba(160, 195, 235, 0.6);
  transform: translateY(-2px);
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
}

.section-title {
  font-size: 1.75rem;
  font-weight: 400;
  margin-bottom: 2.5rem;
  text-align: center;
  color: #202124;
}

.loading {
  text-align: center;
  color: #5f6368;
  font-weight: 300;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 2.5rem;
}

.pagination {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  justify-content: center;
  margin-top: 2rem;
}

.page-button {
  background: #ffffff;
  color: #1a73e8;
  border: 1px solid rgba(26, 115, 232, 0.25);
  border-radius: 9999px;
  padding: 0.8rem 1rem;
  cursor: pointer;
  transition: all 0.2s ease;
}

.page-button:hover:not(:disabled) {
  background: #e8f0fe;
}

.page-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-button.active {
  background: #1a73e8;
  color: #ffffff;
  border-color: #1a73e8;
}

.pagination-summary {
  text-align: center;
  color: #5f6368;
  font-size: 0.95rem;
  margin-top: 1rem;
}
</style>
