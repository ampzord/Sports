<template>
  <div class="flex flex-col items-center justify-center py-20 text-center">
    <h1 class="text-6xl font-bold text-blue-600 mb-4">404</h1>
    <p class="text-xl text-gray-600 mb-8">Page not found</p>
    <router-link
      to="/home"
      class="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded font-semibold transition cursor-pointer"
    >
      Go Home
    </router-link>
  </div>
</template>
