import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Device } from 'app/models/device.model';
import { RequestCreate } from 'app/models/requestcreate.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'devices-cmp',
    moduleId: module.id,
    templateUrl: 'devices.component.html',
    styleUrls: ['devices.component.scss']
})

export class DevicesComponent {
    public modalTitle: String = 'New device';
    public showModal: Boolean = false;
    public devices: Device[] = [];
    public filterType: String = 'All';
    public search: String = '';
    public currentDevice: Device;
    public deviceReqs: RequestCreate[] = [];
    private srcDevices: Device[] = [];

    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        this.getDevices();
        this.getDeviceReqs();
    }

    openModal() {
        this.modalTitle = "New device";
        this.showModal = true;
        this.currentDevice = {
            deviceID: '',
            deviceName: '',
            deviceType: 'Screen',
            staffID: '',
            staffName: '',
            condition: '',
            publicIP: ''
        }
    }

    closeModal() {
        this.showModal = false;
    }

    editDevice(device: Device) {
        this.modalTitle = "Edit device information";
        this.showModal = true;
        this.currentDevice = {
            deviceID: device.deviceID,
            deviceName: device.deviceName,
            deviceType: device.deviceType,
            staffID: device.staffID,
            staffName: device.staffName,
            condition: device.condition,
            publicIP: device.publicIP
        }
    }

    getDevices() {
        const url = `https://localhost:7243/api/Device/get-all`;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.srcDevices = res;
                    this.getFilterDevice();
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    process() {
        if (this.modalTitle.includes('New')) {
            this.newDevice();
        }
        else {
            this.updateDevice();
        }
    }

    getDeviceReqs() {
        const url = `https://localhost:7243/api/Requests/get-device-reqs`;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.deviceReqs = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    newDevice() {
        if (this.currentDevice.deviceID === '' || this.currentDevice.deviceName === '' || this.currentDevice.deviceType === '') {
            this.showAlert('Please fill in all required fields', 'error');
            return;
        }

        const url = `https://localhost:7243/api/Device/new-device`;
        this.httpClient.post(url, {
            deviceID: this.currentDevice.deviceID,
            deviceName: this.currentDevice.deviceName,
            deviceType: this.currentDevice.deviceType,
            staffID: this.currentDevice.staffID === '' ? 'Empty' : this.currentDevice.staffID,
            condition: this.currentDevice.condition === '' ? '__' : this.currentDevice.condition,
            publicIP: this.currentDevice.publicIP
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('New device added successfully', 'success');
                    this.getDevices();
                    this.currentDevice = {
                        deviceID: '',
                        deviceName: '',
                        deviceType: 'Screen',
                        staffID: '',
                        staffName: '',
                        condition: '',
                        publicIP: ''
                    }
                },
                error: (error: any) => {
                    console.log(error)
                    this.showAlert(error.error, 'error');
                }
            });
    }

    updateDevice() {
        if (this.currentDevice.deviceID === '' || this.currentDevice.deviceName === '' || this.currentDevice.deviceType === '') {
            this.showAlert('Please fill in all required fields', 'error');
            return;
        }

        const url = `https://localhost:7243/api/Device/update`;
        this.httpClient.put(url, {
            deviceID: this.currentDevice.deviceID,
            deviceName: this.currentDevice.deviceName,
            deviceType: this.currentDevice.deviceType,
            staffID: this.currentDevice.staffID === '' ? 'Empty' : this.currentDevice.staffID,
            condition: this.currentDevice.condition === '' ? '__' : this.currentDevice.condition,
            publicIP: this.currentDevice.publicIP
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Device updated successfully', 'success');
                    this.getDevices();
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert(error.error, 'error');
                }
            });
    }

    getFilterDevice() {
        this.devices = this.srcDevices.filter((device: Device) => {
            if (this.filterType === 'Has' && device.staffID === '') {
                return false;
            }
            if (this.filterType === 'No' && device.staffID !== '') {
                return false;
            }
            if (this.search === '') {
                return true;
            }
            return device.deviceName.toLowerCase().includes(this.search.toLowerCase())
                || device.deviceType.toLowerCase().includes(this.search.toLowerCase())
                || device.deviceID.toLowerCase().includes(this.search.toLowerCase())
                || device.staffName.toLowerCase().includes(this.search.toLowerCase())
        });
        console.log(this.srcDevices);
        console.log(this.devices);
    }

    showAlert(content: string, type: string): void {
        switch (type) {
            case 'info':
                this.toastr.info(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-info alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
            case 'success':
                this.toastr.success(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-success alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
            case 'error':
                this.toastr.success(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-danger alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
        }
    }
}