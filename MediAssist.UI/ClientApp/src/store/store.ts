// stores/myStore.ts
import axiosInstance from '@/Services/Interceptors/axios';
import { defineStore } from 'pinia';



export const useMyStore = defineStore('myStore', {
  state: () => ({
    UserActivityMetrics: {
      Transcriptions: 0,
      AvailableHours: 0,
      SessionDurationLimit: 0,
      RealTimeResults: null,
      PriorityAccessToTheLatestModels: null,
      WatermarkRemoval: true
    }, 
    MediAssistConfigManager: {
      DomainName : null,
      SupportEmail : null
    }  
  }),
  actions: {
    async fetchUserActivityMetrics(userid) {
      try
      {
        const response = await axiosInstance.get('/api/manage/userconfig/', {params: { userid }});  

        this.UserActivityMetrics.Transcriptions                  = response.data.transcriptions;
        this.UserActivityMetrics.AvailableHours                  = response.data.availableHours;
        this.UserActivityMetrics.SessionDurationLimit            = response.data.sessionDurationLimit;
        this.UserActivityMetrics.RealTimeResults                 = response.data.realTimeResults;
        this.UserActivityMetrics.PriorityAccessToTheLatestModels = response.data.priorityAccessToTheLatestModels;
        this.UserActivityMetrics.WatermarkRemoval                = response.data.watermarkRemoval; 
        this.UserActivityMetrics.UserSessionsCount               = response.data.userSessionsCount;   
      } 
      catch (error)
      {
        console.error('Error fetching data');
      }
    },
    async fetchGlobalConfigurations(){
      try
      {
        const response = await axiosInstance.get('/api/Config/get');              

        this.MediAssistConfigManager.DomainName = response.data.domainName;
        this.MediAssistConfigManager.SupportEmail = response.data.supportEmail;
      } 
      catch (error)
      {
        console.error('Error fetching config values');
      }
    },
  },
});

