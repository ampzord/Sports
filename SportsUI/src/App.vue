<template>
  <div class="min-h-screen bg-white">
    <header class="bg-blue-600 text-white py-4 px-6 shadow">
      <div class="max-w-6xl mx-auto flex justify-between items-center">
        <router-link to="/home" class="text-2xl font-bold text-white no-underline flex items-center gap-2">
          ⚽ Sports API
        </router-link>
        <nav class="flex gap-6">
          <router-link to="/home" class="nav-link">Home</router-link>
          <router-link to="/leagues" class="nav-link">Leagues</router-link>
          <router-link to="/teams" class="nav-link">Teams</router-link>
          <router-link to="/players" class="nav-link">Players</router-link>
          <router-link to="/matches" class="nav-link">Matches</router-link>
        </nav>
      </div>
    </header>

    <main class="max-w-6xl mx-auto p-6">
      <router-view v-slot="{ Component }">
        <Transition name="page" mode="out-in">
          <component :is="Component" :key="$route.path" />
        </Transition>
      </router-view>
    </main>
  </div>
</template>

<script setup></script>

<style>
.nav-link {
  color: white;
  font-weight: 600;
  position: relative;
  padding-bottom: 4px;
  transition: color 0.2s ease;
}
.nav-link:hover {
  color: #bfdbfe;
}
.nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 2px;
  background: white;
  border-radius: 9999px;
  transform: scaleX(0);
  transition: transform 0.3s ease;
  transform-origin: center;
}
.nav-link.router-link-active::after {
  transform: scaleX(1);
}

.page-enter-active,
.page-leave-active {
  transition:
    opacity 0.2s ease,
    transform 0.2s ease;
}
.page-enter-from {
  opacity: 0;
  transform: translateY(6px);
}
.page-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
