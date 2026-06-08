<script setup>
import { ref, onMounted } from 'vue'
import Navbar from '@/components/navbar.vue'
import ProductAdminPanel from '@/components/ProductAdminPanel.vue'
import ProductInfoDetails from '@/components/ProductInfoDetails.vue'
import { productService } from '@/services/ProductService'
import { jwtDecode } from 'jwt-decode'

const props = defineProps({
  productId: {
    type: String, 
    required: true
  }
})

const product = ref(null)
const isAdmin = ref(false)

const fetchProductData = async (id) => {
  try {
    const response = await productService.getSingleProduct(id)
    product.value = response
  } catch(error) {
    alert("Nie udało się załadować produktu:", error)
  }
}

const checkUserRole = () => {
  const token = localStorage.getItem('token')
  if (token) {
    try {
      const decodedPayload = jwtDecode(token)
      isAdmin.value = decodedPayload.role === 'Admin'
    } catch (error) {
      console.error("Nieprawidłowy token JWT", error)
    }
  }
}

const handleProductUpdated = async () => {
  await fetchProductData(props.productId)
}

onMounted(async () => {
  try {
    await fetchProductData(props.productId)
  } catch (e) {
    console.log('błąd podczas pobierania danych z serwera', e)
  }
  checkUserRole()
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
        <ProductInfoDetails 
          :product="product" 
          :productId="props.productId" 
        />

        <ProductAdminPanel 
          v-if="isAdmin" 
          :productId="props.productId" 
          :product="product" 
          @product-updated="handleProductUpdated"
        />
      </div>
    </div>

    <div v-else class="loading-state">
      <div class="chrome-spinner"></div>
      <p>Pobieranie danych...</p>
    </div>
  </div>
</template>

<style scoped>
.chrome-page-wrapper {
  background: radial-gradient(circle at 50% 20%, var(--color-gradient-start) 0%, var(--color-gradient-mid) 40%, var(--color-gradient-end) 100%);
  min-height: 100vh;
  color: var(--color-text);
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
}
</style>