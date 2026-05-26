<script setup>
import { ref,onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import { productService } from '@/services/productService'

const products = ref([])
const fetchProducts = async () => {
  try {

    products.value = await productService.getProductList()
  } catch (error) {
    console.error("Nie udało się załadować produktów:", error)
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
          <h2 class="brand">Fashion Store</h2>
          <p class="tagline">Zapraszamy</p>
          <a class="cta" href="#products">Zobacz moją kolekcję</a>
        </div>
      </header>

      <main class="container">
        <section id="products" class="products-section">
          <h3 class="section-title">Wybrane produkty</h3>

          <p v-if="products.length === 0" class="loading">Ładowanie produktów lub brak danych...</p>

         <div v-else class="grid">
            <RouterLink 
              v-for="product in products" 
              :key="product.id" 
              :to="{ name: 'product-detail', params: { productId: product.id } }" 
              class="card"
              tag="article" 
            >
              <div class="card-media">
                <img v-if="product.imageUrl" :src="product.imageUrl" :alt="product.title" />
                <div v-else class="placeholder">Brak zdjęcia</div>
              </div>
              
              <div class="card-body">
                <h4 class="product-title">{{ product.title }}</h4>
                <p class="product-desc">{{ product.description }}</p>
                <div class="card-footer">
                  <span class="price">{{ product.price ? product.price + ' zł' : '' }}</span>
                  <button class="buy" @click.stop="cartActions.addToCart(product)">Do koszyka</button>
                </div>
              </div>
              <div class="silver-edge"></div>
            </RouterLink>
          </div>
        </section>
      </main>
    </div>
  </template>


  <style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600&display=swap');

.home-root {

  font-family: 'Outfit', sans-serif;
  /* Rozmyty, jasnoniebieski gradient w tle */
  background: radial-gradient(circle at 50% 20%, #dcecfa 0%, #edf4fc 40%, #ffffff 100%);
  min-height: 100vh;
  color: #3c4043;
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

.card {
  background: #ffffff;
  border-radius: 32px; 
  text-decoration: none;
  color: inherit;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  position: relative;

  box-shadow: 0 10px 30px rgba(175, 205, 240, 0.35);
  transition: box-shadow 0.3s ease, transform 0.3s ease;
}

.card:hover {
  box-shadow: 0 16px 40px rgba(175, 205, 240, 0.55);
  transform: translateY(-4px);
}

.card-media {
  height: 220px;
  background: #f4f8fd; 
  display: flex;
  align-items: center;
  justify-content: center;
}

.card-media img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.placeholder {
  color: #a1c2fa;
  font-weight: 400;
  font-size: 0.9rem;
}

.card-body {
  padding: 1.75rem;
  display: flex;
  flex-direction: column;
  flex-grow: 1;
}

.product-title {
  font-size: 1.2rem;
  font-weight: 500;
  margin: 0 0 0.5rem 0;
  color: #202124;
}

.product-desc {
  font-size: 0.95rem;
  font-weight: 300;
  color: #5f6368;
  margin-bottom: 2rem;
  line-height: 1.6;
  flex-grow: 1;
}

.card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.price {
  font-size: 1.15rem;
  font-weight: 500;
  color: #202124;
}

.buy {
  background: #e8f0fe; 
  color: #1a73e8; 
  border: none;
  padding: 0.6rem 1.25rem;
  border-radius: 24px;
  font-size: 0.9rem;
  font-weight: 500;
  font-family: inherit;
  cursor: pointer;
  transition: background 0.2s ease;
}

.buy:hover {
  background: #d2e3fc;
}

.silver-edge {
  height: 3px;
  background: linear-gradient(90deg, transparent, rgba(160, 195, 235, 0.3), transparent);
  width: 100%;
  position: absolute;
  bottom: 0;
}
</style>
