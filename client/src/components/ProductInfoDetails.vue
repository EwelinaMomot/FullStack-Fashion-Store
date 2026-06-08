<script setup>
import { ref } from 'vue'
import { cartActions } from '@/store/cart'

const props = defineProps({
  productId: { type: String, required: true },
  product: { type: Object, required: true }
})

const isVisible = ref(false)

const handleAddToCart = (product) => {
  cartActions.addToCart(product)
  showWithTimeout()
}

const showWithTimeout = () => {
  isVisible.value = true
  setTimeout(() => { isVisible.value = false }, 3000)
}
</script>

<template>
  <div>
    <div class="categories-tags" v-if="product.productCategoryList && product.productCategoryList.length">
      <span 
        v-for="(category, index) in product.productCategoryList" 
        :key="index" 
        class="category-badge"
      >
        {{ category }}
      </span>
    </div>

    <h1 class="product-title">{{ product.title }}</h1>
    <div class="product-meta">
      <span class="meta-item">ID: {{ productId }}</span>
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
      <button class="btn-chrome-primary" @click="handleAddToCart(product)">
        <span class="btn-text">Dodaj do koszyka</span>
        <div class="btn-glow"></div>
      </button>
    </div>
    <span v-if="isVisible" class="added-notification">Dodano do koszyka</span>
  </div>
</template>

<style scoped>
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

.added-notification {
  display: inline-block;
  margin-top: 10px;
  color: #1a73e8;
  font-weight: 500;
}

@media (max-width: 900px) {
  .product-title {
    font-size: 2rem;
  }
}
</style>