<script setup>
import { ref,onMounted } from 'vue'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import ProductCard from '@/components/ProductCard.vue'
import { productService } from '@/services/ProductService'

const products = ref([])
const fetchProducts = async () => {
  try {

    products.value = await productService.getProductList()
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
</style>
