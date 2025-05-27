<template>
    <div class="hamburger-menu">
      <!-- Hamburger Icon -->
      <button @click="toggleMenu" class="hamburger-icon">
        <span :class="{ open: isOpen }"></span>
        <span :class="{ open: isOpen }"></span>
        <span :class="{ open: isOpen }"></span>
      </button>
  
      <!-- Menu Items -->
      <nav v-if="isOpen" class="menu">
        <ul>
          <li><a href="dashboard">Home</a></li>
          <li><a href="settings">Settings</a></li>          
        </ul>
      </nav>
    </div>
  </template>

  <script setup>
  import { ref } from 'vue'
  
  const isOpen = ref(false)
  
  function toggleMenu() {
    isOpen.value = !isOpen.value
  }
  </script>
  <style scoped>
  .hamburger-menu {
    display: flex;
    align-items: center;
  }
  
  .hamburger-icon {
    display: flex;
    flex-direction: column;
    gap: 5px;
    cursor: pointer;
    border: none;
    background: transparent;
  }
  
  .hamburger-icon span {
    display: block;
    width: 25px;
    height: 3px;
    background-color: #333;
    transition: transform 0.3s ease, opacity 0.3s ease;
  }
  
  /* Transformations for the open state */
  .hamburger-icon span.open:nth-child(1) {
    transform: translateY(8px) rotate(45deg);
  }
  
  .hamburger-icon span.open:nth-child(2) {
    opacity: 0;
  }
  
  .hamburger-icon span.open:nth-child(3) {
    transform: translateY(-8px) rotate(-45deg);
  }
  
  /* Menu styling */
  .menu {
    display: flex;
    flex-direction: column;
    background-color: #f7f7f7;
    position: absolute;
    top: 87px;
    left: 0;
    width: 80%;
    padding: 20px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
  
  .menu ul {
    list-style: none;
    padding: 0;
    margin: 0;
  }
  
  .menu li {
    margin: 10px 0;
  }
  
  .menu a {
    text-decoration: none;
    color: #333;
    font-size: 1.2em;
  }
  </style>
  
  
  