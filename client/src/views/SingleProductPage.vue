<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const props = defineProps({
  productId: {
    type: String, 
    required: true
  }
})

const product = ref(null)

const fetchProductData = async (id) => {
  const response = await axios.get(`https://localhost:7154/api/products/${id}`)
  product.value = response.data
  console.log('product response:', response.data)
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
            Added: {{ new Date(product.creationDate).toLocaleDateString() }}
          </span>
        </div>

        <div class="divider-line"></div>

        <div class="description-container">
          <h3 class="section-subtitle">Description</h3>
          <p class="product-description">{{ product.description }}</p>
        </div>

        <div class="divider-line"></div>

        <div class="actions-container">
          <button class="btn-chrome-primary">
            <span class="btn-text">Add to Cart</span>
            <div class="btn-glow"></div>
          </button>
        </div>

        <div class="admin-panel-box">
          <h4 class="admin-title">Admin Management</h4>
          <p class="admin-desc">As an administrator, you can permanently update or softly discard this item from the warehouse.</p>
          <div class="admin-actions">
            <button class="btn-admin-edit">Edit Product</button>
            <button class="btn-admin-delete">Delete Item</button>
          </div>
        </div>

      </div>
    </div>

    <div v-else class="loading-state">
      <div class="chrome-spinner"></div>
      <p>Retrieving metallic data...</p>
    </div>
  </div>
</template>

<style scoped>

.chrome-page-wrapper {
  background-color: #ffffff;
  color: #000000;
  min-height: 100vh;
  padding: 4rem 2rem;
  font-family: 'Inter', 'Poppins', sans-serif;
  display: flex;
  justify-content: center;
}

.product-container {
  max-width: 1200px;
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 4rem;
}

@media (max-width: 768px) {
  .product-container {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
}


.image-section {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.chrome-card {
  position: relative;
  background: #f8f9fa;
  border: 1px solid #e2e8f0;
  border-radius: 0px; 
  padding: 1rem;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.03);
  overflow: hidden;
}


.metallic-border-overlay {
  position: absolute;
  top: 0; left: 0; right: 0; bottom: 0;
  border: 1px solid transparent;
  background: linear-gradient(135deg, #fff 0%, rgba(255,255,255,0) 50%, #000 100%) border-box;
  mask: linear-gradient(#fff 0 0) padding-box, linear-gradient(#fff 0 0);
  -webkit-mask: linear-gradient(#fff 0 0) padding-box, linear-gradient(#fff 0 0);
  -webkit-mask-composite: destination-out;
  mask-composite: exclude;
  opacity: 0.6;
  pointer-events: none;
}

.product-image {
  width: 100%;
  height: auto;
  object-fit: cover;
  filter: grayscale(15%) contrast(105%);
  transition: filter 0.3s ease;
}

.chrome-card:hover .product-image {
  filter: grayscale(0%) contrast(100%);
}


.info-section {
  display: flex;
  flex-direction: column;
}

.categories-tags {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.category-badge {
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  background: #000000;
  color: #ffffff;
  padding: 0.3rem 0.8rem;
  font-weight: 700;
}

.product-title {
  font-size: 2.5rem;
  font-weight: 700;
  letter-spacing: -0.03em;
  margin: 0 0 0.5rem 0;
  color: #000000;
  text-transform: uppercase;
}

.product-meta {
  display: flex;
  gap: 1.5rem;
  font-size: 0.8rem;
  color: #718096;
  letter-spacing: 0.05em;
}

.divider-line {
  height: 1px;
  background: linear-gradient(90deg, #000000 0%, #e2e8f0 100%);
  margin: 2rem 0;
  opacity: 0.8;
}

.section-subtitle {
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.15em;
  color: #718096;
  margin-bottom: 1rem;
}

.product-description {
  font-size: 1rem;
  line-height: 1.7;
  color: #2d3748;
}


.btn-chrome-primary {
  position: relative;
  background: linear-gradient(135deg, #111 0%, #333 100%);
  color: #ffffff;
  border: none;
  padding: 1.2rem 3rem;
  font-size: 0.9rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.2em;
  cursor: pointer;
  width: 100%;
  max-width: 350px;
  transition: all 0.3s ease;
  overflow: hidden;
}

.btn-chrome-primary:hover {
  background: linear-gradient(135deg, #000 0%, #222 100%);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
  transform: translateY(-1px);
}


.admin-panel-box {
  margin-top: auto;
  background: #f8f9fa;
  border-left: 3px solid #000000;
  padding: 1.5rem;
  box-shadow: 0 5px 15px rgba(0,0,0,0.01);
}

.admin-title {
  margin: 0 0 0.5rem 0;
  font-size: 0.9rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-weight: 700;
}

.admin-desc {
  font-size: 0.8rem;
  color: #718096;
  margin-bottom: 1rem;
  line-height: 1.4;
}

.admin-actions {
  display: flex;
  gap: 1rem;
}

.btn-admin-edit, .btn-admin-delete {
  background: transparent;
  padding: 0.6rem 1.2rem;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-admin-edit {
  border: 1px solid #000000;
  color: #000000;
}

.btn-admin-edit:hover {
  background: #000000;
  color: #ffffff;
}

.btn-admin-delete {
  border: 1px solid #e53e3e;
  color: #e53e3e;
}

.btn-admin-delete:hover {
  background: #e53e3e;
  color: #ffffff;
}

/* Loader */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 50vh;
  color: #718096;
  font-size: 0.9rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
}

.chrome-spinner {
  width: 40px;
  height: 40px;
  border: 2px solid #e2e8f0;
  border-top: 2px solid #000000;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
