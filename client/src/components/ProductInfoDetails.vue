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

<style scoped src="./ProductInfoDetails.css"></style>