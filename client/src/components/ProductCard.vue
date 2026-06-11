<script setup>
import { RouterLink } from 'vue-router'

defineProps({
  product: {
    type: Object,
    required: true
  },
  onAddToCart: {
    type: Function,
    required: true
  }
})
</script>

<template>
  <RouterLink 
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
        <button class="buy" @click.stop="onAddToCart(product)">Do koszyka</button>
      </div>
    </div>
    <div class="silver-edge"></div>
  </RouterLink>
</template>

<style scoped src="./ProductCard.css"></style>
