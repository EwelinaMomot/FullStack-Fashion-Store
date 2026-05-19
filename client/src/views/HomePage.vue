<script setup>
import { ref,onMounted } from 'vue'
import axios from 'axios'
import { RouterLink } from 'vue-router'

const products = ref([])

const fetchProducts = async () => {
  try {
    const response = await axios.get('https://localhost:7154/api/products')
    products.value = response.data.products
    console.log(response.data.products)
  } catch (e) {
    console.error("Błąd podczas pobierania produktów:", e)
  }
}

onMounted(() => {
  fetchProducts()
})
</script>

  <template>
    <div class="home-root">
      <header class="hero">
        <div class="hero-inner">
          <h2 class="brand">Fashion Store</h2>
          <p class="tagline">Brand for You</p>
          <a class="cta" href="#products">Zobacz kolekcję</a>
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
                  <button class="buy" @click.stop="addToCart(product)">Do koszyka</button>
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

  :root {
    --bg: #101214;
    --surface: #181b1f;
    --surface-light: #242a30;
    --fg: #f4f4f4;
    --muted: #a3a8ae;
    --accent: #b8bbc0;
    --chrome: linear-gradient(135deg, #20252b 0%, #4c525a 25%, #d7d8db 50%, #4c525a 75%, #1a1d21 100%);
  }

  .home-root {
    min-height: 100vh;
    background: radial-gradient(circle at top left, rgba(255,255,255,0.06), transparent 28%),
      linear-gradient(180deg, #0c0f12 0%, #14181d 52%, #101214 100%);
    color: var(--fg);
    font-family: Inter, ui-sans-serif, system-ui, -apple-system, 'Segoe UI', Roboto, 'Helvetica Neue', Arial;
  }

  .hero {
    background: radial-gradient(circle at 20% 20%, rgba(255,255,255,0.12), transparent 24%),
      linear-gradient(180deg, #111418 0%, #161b20 100%);
    color: #f9fafb;
    padding: 56px 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: inset 0 0 60px rgba(255,255,255,0.04);
  }

  .hero-inner {
    max-width: 1100px;
    text-align: left;
  }

  .brand {
    font-size: 48px;
    margin: 0 0 8px 0;
    font-weight: 700;
    letter-spacing: 1px;
  }

  .tagline {
    margin: 0 0 18px 0;
    color: #d6d6d6;
  }

  .cta {
    display: inline-block;
    padding: 12px 22px;
    color: #f4f4f4;
    text-decoration: none;
    border-radius: 999px;
    font-weight: 600;
    position: relative;
    overflow: hidden;
    background: linear-gradient(135deg, #3e434a 0%, #51575f 20%, #9ba0a6 38%, #5b6067 68%, #23272d 100%);
    border: 1px solid rgba(255,255,255,0.15);
    box-shadow: 0 4px 18px rgba(0,0,0,0.25), inset 0 1px 0 rgba(255,255,255,0.1);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
  }

  .cta::before {
    content: '';
    position: absolute;
    top: -20%;
    left: -30%;
    width: 45%;
    height: 170%;
    background: rgba(255,255,255,0.2);
    transform: rotate(25deg);
    transition: transform 0.55s ease;
    filter: blur(4px);
  }

  .cta:hover {
    transform: translateY(-1px);
    box-shadow: 0 6px 20px rgba(0,0,0,0.3), inset 0 1px 0 rgba(255,255,255,0.13);
  }

  .cta:hover::before {
    transform: translateX(110%) rotate(25deg);
  }

  .cta:active { transform: translateY(1px); }

  .container {
    max-width: 1100px;
    margin: -40px auto 80px;
    padding: 0 20px;
  }

  .products-section {
    background: linear-gradient(180deg, rgba(30,33,38,0.95), rgba(22,25,30,0.98));
    border: 1px solid rgba(255,255,255,0.06);
    padding: 30px;
    border-radius: 24px;
    box-shadow: 0 24px 70px rgba(0,0,0,0.35);
  }

  .section-title {
    margin: 0 0 16px 8px;
    font-size: 20px;
    color: #e7e9eb;
  }

  .loading {
    color: var(--muted);
    padding: 20px 8px;
  }

  .grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
    gap: 18px;
  }

  .card {
  display: flex;
  flex-direction: column;
  text-decoration: none; 
  color: inherit;        

  position: relative;
  background: radial-gradient(circle at top right, rgba(255,255,255,0.08), transparent 32%),
    linear-gradient(180deg, #181b1f 0%, #1f242a 100%);
  border: 1px solid rgba(255,255,255,0.08);
  overflow: hidden;
  border-radius: 28px 8px 28px 8px;
  transition: transform 0.22s ease, box-shadow 0.22s ease;
  padding-bottom: 10px;
}

  .card:hover {
    transform: translateY(-6px) scale(1.01);
    box-shadow: 0 20px 60px rgba(0,0,0,0.35);
  }

  .card-media {
    height: 170px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(180deg, #1b1e22 0%, #22272e 100%);
  }

  .card-media img {
    max-width: 100%;
    max-height: 100%;
    object-fit: cover;
    display: block;
    border-radius: 16px;
  }

  .placeholder {
    color: #7a7e85;
    font-size: 14px;
  }

  .card-body {
    padding: 14px 16px 10px 16px;
  }

  .product-title {
    margin: 0 0 8px 0;
    font-size: 16px;
    font-weight: 600;
    color: #f4f4f4;
  }

  .product-desc {
    margin: 0 0 14px 0;
    color: #9fa5ab;
    font-size: 13px;
    min-height: 36px;
  }

  .card-footer {
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  .price {
    color: #f4f4f4;
    font-weight: 700;
  }

  .buy {
    background: linear-gradient(135deg, #45484d 0%, #606569 20%, #9da1a7 45%, #5c6167 75%, #2b2f34 100%);
    border: 1px solid rgba(255,255,255,0.12);
    color: #f4f4f4;
    padding: 8px 12px;
    border-radius: 999px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
    box-shadow: 0 5px 14px rgba(0,0,0,0.25), inset 0 1px 0 rgba(255,255,255,0.08);
    transition: transform 0.18s ease, box-shadow 0.18s ease;
  }

  .buy::after {
    content: '';
    position: absolute;
    top: -40%;
    left: -30%;
    width: 40%;
    height: 200%;
    background: rgba(255,255,255,0.18);
    transform: rotate(25deg);
    transition: transform 0.35s ease;
  }

  .buy:hover {
    transform: translateY(-1px);
    box-shadow: 0 6px 16px rgba(0,0,0,0.3), inset 0 1px 0 rgba(255,255,255,0.12);
  }

  .buy:hover::after { transform: translateX(80%) rotate(25deg); }

  .buy:active { transform: translateY(1px); }

  .silver-edge {
    position: absolute;
    right: -40px;
    top: 10px;
    width: 80px;
    height: 20px;
    transform: rotate(18deg);
    background: linear-gradient(90deg, rgba(214,214,214,1), rgba(170,175,183,0.95), rgba(220,220,220,0.98));
    opacity: 0.95;
    filter: blur(0.2px);
    border-radius: 50%;
  }

  @media (max-width: 640px) {
    .hero-inner { text-align: center }
    .brand { font-size: 34px }
    .container { margin-top: 10px }
    .card { border-radius: 20px }
  }
  </style>
