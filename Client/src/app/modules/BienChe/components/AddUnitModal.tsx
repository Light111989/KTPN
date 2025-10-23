import React, { useState, useEffect } from 'react'
import Flatpickr from 'react-flatpickr'
import 'flatpickr/dist/flatpickr.css'
import { createDonVi, updateDonVi } from '../components/settings/_requests'

type Props = {
  show: boolean
  mode: 'add' | 'edit'
  onClose: () => void
  onSubmitSuccess: () => void
  initialData?: any
  linhVucs: any[]
  khois: any[]
}

const AddUnitModal: React.FC<Props> = ({
  show,
  mode,
  onClose,
  onSubmitSuccess,
  initialData,
  linhVucs,
  khois,
}) => {
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
    slHD111: 0,
    effectiveDate: new Date(),
  }

  const [formData, setFormData] = useState(initialForm)

  useEffect(() => {
    if (mode === 'edit' && initialData) {
      setFormData({
        ...initialData,
        effectiveDate: initialData.effectiveDate
          ? new Date(initialData.effectiveDate)
          : new Date(),
      })
    } else {
      setFormData(initialForm)
    }
  }, [mode, initialData, show])

  const handleSave = async () => {
    try {
      const fixedDate = new Date(
        formData.effectiveDate.getFullYear(),
        formData.effectiveDate.getMonth(),
        formData.effectiveDate.getDate(),
        12,
        0,
        0
      )

      const payload = {
        ...formData,
        effectiveDate: fixedDate.toISOString(),
      }

      if (mode === 'add') {
        await createDonVi(payload)
      } else {
        await updateDonVi(payload.id, payload)
      }

      onClose()
      onSubmitSuccess()
    } catch (err) {
      console.error(err)
      alert('Lỗi khi lưu!')
    }
  }

  if (!show) return null

  return (
    <div className="modal fade show d-block" tabIndex={-1} role="dialog">
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              {mode === 'add' ? 'Thêm Đơn Vị' : 'Chỉnh Sửa Đơn Vị'}
            </h5>
            <button
              type="button"
              className="btn-close"
              onClick={onClose}
              aria-label="Close"
            ></button>
          </div>

          <div className="container p-4">
            <div className="row g-3">
              {/* Tên đơn vị */}
              <div className="col-md-12">
                <label className="form-label">Tên đơn vị</label>
                <input
                  type="text"
                  className="form-control form-control-solid"
                  value={formData.tenDonVi}
                  onChange={(e) =>
                    setFormData({ ...formData, tenDonVi: e.target.value })
                  }
                />
              </div>

              {/* Lĩnh vực */}
              <div className="col-md-6">
                <label className="form-label">Lĩnh vực</label>
                <select
                  className="form-select form-select-solid"
                  value={formData.linhVucId}
                  onChange={(e) =>
                    setFormData({ ...formData, linhVucId: e.target.value })
                  }
                >
                  <option value="">-- Chọn lĩnh vực --</option>
                  {linhVucs.map((lv) => (
                    <option key={lv.linhVucId} value={lv.linhVucId}>
                      {lv.tenLinhVuc}
                    </option>
                  ))}
                </select>
              </div>

              {/* Khối */}
              <div className="col-md-6">
                <label className="form-label">Khối</label>
                <select
                  className="form-select form-select-solid"
                  value={formData.khoiId}
                  onChange={(e) =>
                    setFormData({ ...formData, khoiId: e.target.value })
                  }
                >
                  <option value="">-- Chọn Khối --</option>
                  {khois.map((k) => (
                    <option key={k.khoiId} value={k.khoiId}>
                      {k.tenKhoi}
                    </option>
                  ))}
                </select>
              </div>

              {/* Số quyết định */}
              <div className="col-md-12">
                <label className="form-label">Số quyết định</label>
                <input
                  type="text"
                  className="form-control form-control-solid"
                  value={formData.soQuyetDinh}
                  onChange={(e) =>
                    setFormData({ ...formData, soQuyetDinh: e.target.value })
                  }
                />
              </div>

              {/* Ngày hiệu lực */}
              <div className="col-md-4">
                <label className="form-label">Thời Gian QĐ</label>
                <Flatpickr
                  value={formData.effectiveDate}
                  onChange={([date]) => {
                    setFormData((prev) => ({
                      ...prev,
                      effectiveDate: new Date(date.setHours(0, 0, 0, 0)),
                    }))
                  }}
                  className="form-control form-control-solid"
                  options={{
                    dateFormat: 'd-m-Y',
                  }}
                />
              </div>

              {/* Các số lượng */}
              {[
                ['slVienChuc', 'Số lượng viên chức'],
                ['slHopDong', 'Số lượng hợp đồng'],
                ['slHopDongND', 'Số lượng hợp đồng ND'],
                ['slBoTri', 'Số lượng bố trí'],
                ['slGiaoVien', 'Số lượng giáo viên'],
                ['slQuanLy', 'Số lượng quản lý'],
                ['slNhanVien', 'Số lượng nhân viên'],
                ['slHD111', 'Số lượng HD111'],
              ].map(([field, label]) => (
                <div className="col-md-4" key={field}>
                  <label className="form-label">{label}</label>
                  <input
                    type="number"
                    className="form-control form-control-solid"
                    value={(formData as any)[field]}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        [field]: Number(e.target.value),
                      })
                    }
                  />
                </div>
              ))}
            </div>
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn btn-secondary"
              onClick={onClose}
            >
              Hủy
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={handleSave}
            >
              {mode === 'add' ? 'Thêm' : 'Cập nhật'}
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}

export default AddUnitModal
