<template>
    <transition name="fade">
      <div v-if="showPotentialDiagnosisPopup" class="diagnosis-popup">
        <div class="popup-content">
          <div class="diagnosis-icon">
            <img src="@/assets/potential-diagnosis-popup-icon.png" alt="Potential Diagnosis Popup Icon" class="popup-icon" />
          </div>
          <p class="diagnosis-tooltip-text">
            If you prefer not to use the AI-powered <b>Potential Diagnosis</b> feature, you can easily switch it off before starting the session.
          </p>
          <button @click="closePopup" class="popup-button">Okay</button>
        </div>
        <div>
            <img src="@/assets/downarrow-diagnosispopup.png" alt="Potential Diagnosis Popup Down Arrow Icon" class="down-arrow" />
        </div>
      </div>
    </transition>
  </template>
  
  <script setup>
    import { ref, onMounted, defineEmits } from "vue";

    const emit = defineEmits(["closePopup"]);
    const showPotentialDiagnosisPopup = ref(true);

    onMounted(() => {
    setTimeout(() => {
        showPotentialDiagnosisPopup.value = false;
        emit("closePopup");
    }, 7000);
    });

    const closePopup = () => {
    showPotentialDiagnosisPopup.value = false;
    emit("closePopup");
    };
  </script>
  
  <style>
  .diagnosis-popup {
    position: absolute;
    top: 375px;
    right: 80px;
    width: 250px;
    background: white;
    border-radius: 8px;
    box-shadow: 0px 4px 10px dodgerblue;
    padding-bottom: 10px;
    animation: slideInLeft 0.5s ease-out;
  }

  .diagnosis-icon{
    background: #E5F1FB;
    width: 250px;
    border-top-left-radius: 8px;
    border-top-right-radius: 8px;
  }

  .diagnosis-tooltip-text{
    padding-right: 15px;
    padding-left: 15px;
    text-align: center;
    font-size: smaller;
  }
 
  .popup-content {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
  
  .popup-icon {
    width: 200px;
    height: 120px;
  }

  .popup-button {
    padding: 8px 20px;
    border: none;
    background: #007bff;
    color: white;
    border-radius: 8px;
    cursor: pointer;
    position: relative;
    left: 70px;
  }
  
  .popup-button:hover {
    background: #0056b3;
  }

  .down-arrow{
    display: flex;
    justify-content: end;
    position: absolute;
    bottom: -16px;
    right: 10px;
    width: 25px;
    height: 15px;
  }

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

  
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.3s;
  }
  
  .fade-enter,
  .fade-leave-to {
    opacity: 0;
  }
  </style>
  