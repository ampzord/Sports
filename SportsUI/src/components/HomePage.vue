<template>
  <div class="py-8">
    <h1 class="text-3xl font-bold text-blue-600 mb-8 text-center">Dashboard</h1>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <router-link
        to="/leagues"
        class="bg-white border-2 border-blue-200 p-6 rounded-lg hover:border-blue-500 hover:shadow-lg transition-all text-center"
      >
        <h2 class="text-xl font-bold text-blue-600">Leagues</h2>
        <p class="mt-2 text-gray-500 text-sm">Manage leagues</p>
      </router-link>
      <router-link
        to="/teams"
        class="bg-white border-2 border-blue-200 p-6 rounded-lg hover:border-blue-500 hover:shadow-lg transition-all text-center"
      >
        <h2 class="text-xl font-bold text-blue-600">Teams</h2>
        <p class="mt-2 text-gray-500 text-sm">Manage teams</p>
      </router-link>
      <router-link
        to="/players"
        class="bg-white border-2 border-blue-200 p-6 rounded-lg hover:border-blue-500 hover:shadow-lg transition-all text-center"
      >
        <h2 class="text-xl font-bold text-blue-600">Players</h2>
        <p class="mt-2 text-gray-500 text-sm">Manage players</p>
      </router-link>
      <router-link
        to="/matches"
        class="bg-white border-2 border-blue-200 p-6 rounded-lg hover:border-blue-500 hover:shadow-lg transition-all text-center"
      >
        <h2 class="text-xl font-bold text-blue-600">Matches</h2>
        <p class="mt-2 text-gray-500 text-sm">Manage matches</p>
      </router-link>
    </div>
  </div>
</template>
