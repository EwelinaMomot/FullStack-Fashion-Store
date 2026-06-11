import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '../views/HomePage.vue'
import CartView from '../views/ShoppingCartPage.vue'
import SingleProductView from '@/views/SingleProductPage.vue'
import AdminPageView from '@/views/AdminPageView.vue'
import UserLoginPageView from  '@/views/UserRegisterPageView.vue'

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
    path: '/admin',
    component: AdminPageView,
  },
  {
    path: '/newuser',
    component: UserLoginPageView,
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
