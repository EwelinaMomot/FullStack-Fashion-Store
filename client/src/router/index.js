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
    path: '/',
    component: HomeView,
  },
  {
    path: '/cart',
    component: CartView,
  },
  {
    path: '/product/:productId',
    component: SingleProductView,
    name: 'product-detail',
    props: true,
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
})

export default router
