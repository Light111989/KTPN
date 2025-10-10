import React, { useState, useEffect } from 'react'
import { createDonVi, updateDonVi, fetchDonVis, fetchKhois, fetchLinhVucs } from '../components/settings/_requests'
import { Modal } from 'bootstrap'

type Props = {
  mode: 'add' | 'edit'
  onSubmitSuccess: () => void // callback để refresh danh sách
  // onSubmit: (data: any) => Promise<void>; // ⚡ bắt buộc phải có
  initialData?: any
  linhVucs: any[]
  khois: any[]
}


const AddUnitModal: React.FC<Props> = ({ mode, initialData, linhVucs, khois, onSubmitSuccess }) => {
  const initialForm = {
    id: '',
    tenDonVi: '',
    linhVuc: '',
    linhVucId: '',
    khoiId: '',
    khoi: '',
    slVienChuc: 0,
    slHopDong: 0,
    slHopDongND: 0,
    slBoTri: 0,
    soQuyetDinh: '',
    slGiaoVien: 0,
    slQuanLy: 0,
    slNhanVien: 0,
  }
  const [formData, setFormData] = useState(initialForm)

  useEffect(() => {
    if (mode === 'edit' && initialData) {
      setFormData(initialData);        // Load dữ liệu record khi Edit
    } else if (mode === 'add') {
      setFormData(initialForm);        // Reset form khi Add
    }
  }, [mode, initialData]);

  const handleSave = async () => {
    try {
      if (mode === 'add') {
        await createDonVi(formData)
      } else {
        await updateDonVi(formData.id, formData)
      }
      closeModal()
      onSubmitSuccess()
    } catch (err) {
      console.error(err)
      alert('Lỗi khi lưu!')
    }
  }
  // const handleSave = async () => {
  //   try {
  //     if (mode === 'add') {
  //       await createDonVi(formData)
  //       alert('Thêm đơn vị thành công!')
  //     } else {
  //       await updateDonVi(formData.id, formData)
  //       alert('Cập nhật đơn vị thành công!')
  //     }

  //     const modalEl = document.getElementById('kt_modal_1')
  //     if (modalEl) {
  //       const modal = Modal.getInstance(modalEl) || new Modal(modalEl)
  //       modal.hide()
  //     }

  //     onSubmitSuccess()
  //   } catch (err: any) {
  //     console.error(err)
  //     alert((mode === 'add' ? 'Thêm' : 'Cập nhật') + ' thất bại: ' + err.message)
  //   }
  // }

  const openModal = () => {
    const modalEl = document.getElementById('kt_modal_1')
    if (modalEl) {
      const modal = new Modal(modalEl) // luôn tạo instance mới
      modal.show()
    }
  }

  // ✅ Hàm đóng modal
  const closeModal = () => {
    const modalEl = document.getElementById('kt_modal_1')
    if (modalEl) {
      const modal = Modal.getInstance(modalEl)
      modal?.hide()
    }
  }
  // // Reset form mỗi khi modal đóng
  // useEffect(() => {
  //   const modalEl = document.getElementById('kt_modal_1')
  //   if (!modalEl) return

  //   modalEl.addEventListener('hidden.bs.modal', () => {
  //     setFormData(initialForm)
  //   })

  //   return () => {
  //     modalEl.removeEventListener('hidden.bs.modal', () => {
  //       setFormData(initialForm)
  //     })
  //   }

  // }, [])

  return (
    <div className="modal fade" tabIndex={-1} id="kt_modal_1">
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">  {mode === 'add' ? 'Thêm Đơn Vị' : 'Chỉnh Sửa Đơn Vị'}</h5>
            <button
              type="button"
              className="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>

          <div className="container p-4">
            <div className="row g-3">
              <div className="col-md-12">
                <label className="form-label">Tên đơn vị</label>
                <input
                  type="text"
                  className="form-control form-control-solid"
                  value={formData.tenDonVi}
                  onChange={(e) => setFormData({ ...formData, tenDonVi: e.target.value })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Lĩnh vực</label>
                <select
                  className="form-select form-select-solid"
                  value={formData.linhVucId}
                  onChange={(e) => setFormData({ ...formData, linhVucId: e.target.value })}
                >
                  <option value="">-- Chọn lĩnh vực --</option>
                  {linhVucs.map((lv) => (
                    <option key={lv.linhVucId} value={lv.linhVucId}>
                      {lv.tenLinhVuc}
                    </option>
                  ))}
                </select>
              </div>
              <div className="col-md-6">
                <label className="form-label">Khối</label>
                <select
                  className="form-select form-select-solid"
                  value={formData.khoiId}
                  onChange={(e) => setFormData({ ...formData, khoiId: e.target.value })}
                >
                  <option value="">-- Chọn Khối --</option>
                  {khois.map((k) => (
                    <option key={k.khoiId} value={k.khoiId}>
                      {k.tenKhoi}
                    </option>
                  ))}
                </select>
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng viên chức</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slVienChuc}
                  onChange={(e) => setFormData({ ...formData, slVienChuc: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng hợp đồng</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slHopDong}
                  onChange={(e) => setFormData({ ...formData, slHopDong: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng hợp đồng ND</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slHopDongND}
                  onChange={(e) => setFormData({ ...formData, slHopDongND: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng bố trí</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slBoTri}
                  onChange={(e) => setFormData({ ...formData, slBoTri: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số quyết định</label>
                <input
                  type="text"
                  className="form-control form-control-solid"
                  value={formData.soQuyetDinh}
                  onChange={(e) => setFormData({ ...formData, soQuyetDinh: e.target.value })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng giáo viên</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slGiaoVien}
                  onChange={(e) => setFormData({ ...formData, slGiaoVien: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng quản lý</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slQuanLy}
                  onChange={(e) => setFormData({ ...formData, slQuanLy: Number(e.target.value) })}
                />
              </div>

              <div className="col-md-6">
                <label className="form-label">Số lượng nhân viên</label>
                <input
                  type="number"
                  className="form-control form-control-solid"
                  value={formData.slNhanVien}
                  onChange={(e) => setFormData({ ...formData, slNhanVien: Number(e.target.value) })}
                />
              </div>
            </div>
          </div>

          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">
              Hủy
            </button>
            <button type="button" className="btn btn-primary" onClick={handleSave}>
              {(mode === 'add' ? 'Thêm' : 'Cập nhật')}
            </button>
          </div>
        </div>
      </div>
    </div>
  )

}

export default AddUnitModal
