<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { cartActions } from '@/store/cart'
import Navbar from '@/components/navbar.vue'
import { productService } from '@/services/ProductService'

const props = defineProps({
  productId: {
    type: String, 
    required: true
  }
})

const product = ref(null)

const fetchProductData = async (id) => {
  try{

    product.value=  productService.getSingleProduct(id)
  }catch(error){  alert("Nie udało się załadować produktu:", error)}


}

const handleAddToCart =(product)=>{
  cartActions.addToCart(product)
}

const isVisible = ref(false)


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
        <div class="categories-tags" v-if="product.categories && product.categories.length">
          <span 
            v-for="category in product.categories" 
            :key="category.id" 
            class="category-badge"
          >
            {{ category.name }}
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

        <div class="admin-panel-box">
          <h4 class="admin-title">Panel administratora</h4>
          <p class="admin-desc">Jako administrator możesz trwale zaktualizować lub usunąć ten produkt ze stanów magazynowych.</p>
          <div class="admin-actions">
            <button class="btn-admin-edit">Edytuj produkt</button>
            <button class="btn-admin-delete">Usuń produkt</button>
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
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600&display=swap');


.chrome-page-wrapper {
  font-family: 'Outfit', sans-serif;
  background: radial-gradient(circle at 50% 20%, #dcecfa 0%, #edf4fc 40%, #ffffff 100%);
  min-height: 100vh;
  color: #3c4043;
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


.btn-chrome-primary {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: #ffffff;
  color: #3c4043;
  border: none;
  padding: 1rem 2.5rem;
  border-radius: 40px;
  font-size: 1rem;
  font-weight: 500;
  font-family: inherit;
  cursor: pointer;
  box-shadow: 0 8px 24px rgba(160, 195, 235, 0.4);
  transition: all 0.3s ease;
  width: auto;
}

.btn-chrome-primary:hover {
  box-shadow: 0 12px 32px rgba(160, 195, 235, 0.6);
  transform: translateY(-2px);
  color: #1a73e8; }

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

.btn-admin-edit, .btn-admin-delete {
  padding: 0.6rem 1.25rem;
  border-radius: 24px;
  font-family: inherit;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  border: none;
  transition: background 0.2s ease;
}

.btn-admin-edit {
  background: #e8f0fe;
  color: #1a73e8;
}

.btn-admin-edit:hover {
  background: #d2e3fc;
}

.btn-admin-delete {
  background: #fce8e6;
  color: #d93025;
}

.btn-admin-delete:hover {
  background: #fad2cf;
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