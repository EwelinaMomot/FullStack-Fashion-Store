import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '../views/HomePage.vue'
import CartView from '../views/ShoppingCartPage.vue'
import SingleProductView from '@/views/SingleProductPage.vue'

const routes = [
  {
    path: '/home',
    component: HomeView,
  },
  {
    path: '/cart',
    component: CartView,
  },
  {
    path: '/product/:productId',
    component: SingleProductView,
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
})

export default router
