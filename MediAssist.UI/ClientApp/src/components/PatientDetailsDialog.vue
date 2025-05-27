<template>
  <div class="dialog-container">
    <div class="dialog-content">
      <div class="message-container">
        <div class="icon-container">
          <i class="info-icon"
            ><img
              src="@/assets/disclaimer.png"
              alt="Disclaimer Icon"
              class="disclaimer-icon"
          /></i>
        </div>
        <p class="message">
          Complete the session and fill in patient details to generate the report.
        </p>
      </div>
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.disclaimer-icon {
  width: 17px;
  height: 19px;
  margin-top: 8px;
}
.dialog-container {
  position: absolute;
  right: 3%;
  display: flex;
  justify-content: center;
  align-items: center;
  top: 80%;
}

.dialog-content {
  border-radius: 8px;
  border: 1px solid #f5f5f5;
  background: var(--bg-white, #fff);
  box-shadow: 0px 4px 10px 0px rgba(159, 177, 201, 0.25);
  display: flex;
  width: auto;
  padding: 5px 5px;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  gap: 10px;
  z-index: 2;
  max-width: 260px;
}

.message-container {
  display: flex;
  align-items: flex-start;
}

.icon-container {
  margin-right: 5px;
  flex-shrink: 0;
}

.info-icon {
  color: white;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-style: normal;
  font-weight: bold;
}

.message {
  margin: 0;
  font-size: 13px;
  line-height: 1.4;
}

.button-container {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.cancel-button,
.confirm-button {
  color: #353535;
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 120%;
  border-radius: 4px;
  border: 1px solid #e5e5e5;
  background: var(--bg-white, #fff);
  display: flex;
  padding: 8.5px 10px;
  justify-content: center;
  align-items: center;
  gap: 6px;
  flex: 1 0 0;
  align-self: stretch;
  cursor: pointer;
  margin-left: 10px;
  width: 129px;
  height: 38px;
}

.cancel-button {
  background-color: #f8f9fa;
  color: #212529;
}

.confirm-button {
  background-color: #007bff;
  color: white;
}
</style>
