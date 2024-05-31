import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Staff } from 'app/models/staff.model';
import { ToastrService } from 'ngx-toastr';
import { title } from 'process';
import { start } from 'repl';

@Component({
    selector: 'userinfos-cmp',
    moduleId: module.id,
    templateUrl: 'userinfos.component.html',
    styleUrls: ['userinfos.component.scss']
})

export class UserInfosComponent {
    public modalTitle: String = 'Thêm nhân viên';
    public showModal: Boolean = false;
    public staffs: Staff[] = [];
    private srcStaffs: Staff[] = [];
    public filter: String = '';
    public allDivisions: String[] = [];
    public lastestStaffID: String;
    public currentStaff: Staff;
    public pw: String = '';

    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        this.getAllStaff();
    }

    openModal() {
        this.modalTitle = 'Thêm nhân viên';
        this.showModal = true;
        this.currentStaff = {
            staffID: this.lastestStaffID.toString(),
            staffName: '',
            gender: 'Nam',
            dateBirth: '',
            phone: '',
            address: '',
            startWork: '',
            startFullTime: '',
            title: 'Staff',
            level: 'Fresher',
            companyEmail: '',
            personalEmail: '',
            avatarURL: '',
            managerID: '',
            managerName: '',
            divisionID: this.allDivisions[0].toString(),
            roles: 'Staff',
            allDivision: '',
            bhxh: '',
            bhyt: '',
            bhtn: '',
            maSoThue: '',
            hdld: '',
            soHDLD: '',
            bankAccount: '',
            bankName: ''
        }
        this.pw = '';
    }

    editUser(staff: Staff) {
        this.modalTitle = 'Sửa thông tin nhân viên';
        this.showModal = true;
        this.currentStaff = {
            staffID: staff.staffID,
            staffName: staff.staffName,
            gender: staff.gender,
            dateBirth: staff.dateBirth,
            phone: staff.phone,
            address: staff.address,
            startWork: staff.startWork,
            startFullTime: staff.startFullTime,
            title: staff.title,
            level: staff.level,
            companyEmail: staff.companyEmail,
            personalEmail: staff.personalEmail,
            avatarURL: staff.avatarURL,
            managerID: staff.managerID,
            managerName: staff.managerName,
            divisionID: staff.divisionID,
            roles: staff.roles,
            allDivision: staff.allDivision,
            bhxh: staff.bhxh,
            bhyt: staff.bhyt,
            bhtn: staff.bhtn,
            maSoThue: staff.maSoThue,
            hdld: staff.hdld,
            soHDLD: staff.soHDLD,
            bankAccount: staff.bankAccount,
            bankName: staff.bankName
        }
    }

    closeModal() {
        this.showModal = false;
    }

    onCheckboxChange(checked: boolean, role: string) {
        if (checked) {
            this.currentStaff.roles += '_' + role;
        } else {
            this.currentStaff.roles = this.currentStaff.roles.replace('_' + role, '');
        }
    }

    getAllStaff() {
        this.httpClient.get('http://localhost:5299/api/Staff/get-all')
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.srcStaffs = res;
                    this.allDivisions = this.srcStaffs[0].allDivision.split('_');
                    this.filterStaff();
                    this.getLastestStaffID();
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    filterStaff() {
        this.staffs = this.srcStaffs.filter((staff) => {
            if (this.filter === '') return true;

            return staff.staffName.toLowerCase().includes(this.filter.toLowerCase())
                || staff.staffID.toLowerCase().includes(this.filter.toLowerCase());
        });
    }

    getLastestStaffID() {
        this.lastestStaffID = (Number(this.srcStaffs[this.srcStaffs.length - 1].staffID) + 1).toString();
    }

    hasRole(staff: Staff, role: string) {
        if (role == 'Staff')
            return true;
        return staff.roles.includes(role);
    }

    process() {
        if (this.modalTitle == 'Thêm nhân viên') {
            this.addStaff();
        } else {
            this.updateStaff();
        }
    }

    addStaff() {
        console.log(this.currentStaff);
        if (this.currentStaff.staffName == '' || this.currentStaff.dateBirth == ''
            || this.currentStaff.address == '' || this.currentStaff.startWork == '' || this.pw == '') {
            this.showAlert('Vui lòng điền đầy đủ thông tin!', 'info');
            return;
        }

        this.httpClient.post('http://localhost:5299/api/Account/register', {
            staffID: this.currentStaff.staffID,
            staffName: this.currentStaff.staffName,
            title: this.currentStaff.title,
            level: this.currentStaff.level,
            male: this.currentStaff.gender == 'Nam',
            address: this.currentStaff.address,
            dateBirth: this.currentStaff.dateBirth,
            startWork: this.currentStaff.startWork,
            password: this.pw,
            managerID: this.currentStaff.managerID == '' ? 'Empty' : this.currentStaff.managerID,
            divisionID: this.currentStaff.divisionID,
            roles: this.currentStaff.roles
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Thêm nhân viên thành công!', 'success');
                    this.showModal = false;
                    this.getAllStaff();
                },
                error: (error: any) => {
                    console.log(error);
                }
            });
    }

    updateStaff() {
        this.httpClient.put('http://localhost:5299/api/Staff/hr-update', {
            staffID: this.currentStaff.staffID,
            staffName: this.currentStaff.staffName,
            divisionID: this.currentStaff.divisionID,
            startFullTime: this.currentStaff.startFullTime == '' ? 'Empty' : this.currentStaff.startFullTime,
            title: this.currentStaff.title,
            level: this.currentStaff.level,
            managerID: this.currentStaff.managerID == '' ? 'Empty' : this.currentStaff.managerID,
            bhxh: this.currentStaff.bhxh == '' ? 'Empty' : this.currentStaff.bhxh,
            bhyt: this.currentStaff.bhyt == '' ? 'Empty' : this.currentStaff.bhyt,
            bhtn: this.currentStaff.bhtn == '' ? 'Empty' : this.currentStaff.bhtn,
            maSoThue: this.currentStaff.maSoThue == '' ? 'Empty' : this.currentStaff.maSoThue,
            hdld: this.currentStaff.hdld == '' ? 'Empty' : this.currentStaff.hdld,
            soHDLD: this.currentStaff.soHDLD == '' ? 'Empty' : this.currentStaff.soHDLD,
            roles: this.currentStaff.roles
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Cập nhật thông tin nhân viên thành công!', 'success');
                    this.getAllStaff();
                    this.closeModal();
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert('Đã có lỗi xảy ra!', 'info');
                }
            });
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
        }
    }
}