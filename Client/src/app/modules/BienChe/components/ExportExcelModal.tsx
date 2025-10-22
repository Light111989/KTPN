import React, { useState } from 'react'
import { Modal, Button } from 'react-bootstrap'
import Flatpickr from 'react-flatpickr'
import 'flatpickr/dist/flatpickr.css'

type Props = {
    show: boolean
    onClose: () => void
    linhVucs: any[]
    khois: any[]
    onExport: (filters: any) => void
}

const ExportExcelModal: React.FC<Props> = ({ show, onClose, linhVucs, khois, onExport }) => {
    const [filters, setFilters] = useState({
        fromDate: null as Date | null,
        toDate: null as Date | null,
        khoiId: '',
        linhVucId: '',
        tenDonVi: '',
        type: 'current' // 'current' | 'history'
    })

    const handleExport = () => {
        const payload = {
            ...filters,
            khoiId: filters.khoiId || null,
            linhVucId: filters.linhVucId || null
        }
        onExport(payload)
        onClose()
    }


    return (
        <Modal show={show} onHide={onClose} size="lg" centered>
            <Modal.Header closeButton>
                <Modal.Title>Xuất Excel</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="col-md-12">
                    <label className="form-label">Dữ liệu xuất</label>
                    <div>
                        <div className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="radio"
                                name="type"
                                value="current"
                                checked={filters.type === 'current'}
                                onChange={() => setFilters({ ...filters, type: 'current' })}
                            />
                            <label className="form-check-label">Biên chế hiện tại</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="radio"
                                name="type"
                                value="history"
                                checked={filters.type === 'history'}
                                onChange={() => setFilters({ ...filters, type: 'history' })}
                            />
                            <label className="form-check-label">Lịch sử biên chế</label>
                        </div>
                    </div>
                </div>
                <div className="row g-3">
                    <div className="col-md-6">
                        <label className="form-label">Từ ngày</label>
                        <Flatpickr
                            value={filters.fromDate || undefined}
                            onChange={([date]) => setFilters({ ...filters, fromDate: date })}
                            className="form-control form-control-sm"
                            options={{ dateFormat: 'd-m-Y' }}
                        />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label">Đến ngày</label>
                        <Flatpickr
                            value={filters.toDate || undefined}
                            onChange={([date]) => setFilters({ ...filters, toDate: date })}
                            className="form-control form-control-sm"
                            options={{ dateFormat: 'd-m-Y' }}
                        />
                    </div>

                    <div className="col-md-6">
                        <label className="form-label">Lĩnh vực</label>
                        <select
                            className="form-select form-select-sm"
                            value={filters.linhVucId}
                            onChange={(e) => setFilters({ ...filters, linhVucId: e.target.value })}
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
                            className="form-select form-select-sm"
                            value={filters.khoiId}
                            onChange={(e) => setFilters({ ...filters, khoiId: e.target.value })}
                        >
                            <option value="">-- Chọn khối --</option>
                            {khois.map((k) => (
                                <option key={k.khoiId} value={k.khoiId}>
                                    {k.tenKhoi}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="col-md-12">
                        <label className="form-label">Tên đơn vị</label>
                        <input
                            type="text"
                            className="form-control form-control-sm"
                            value={filters.tenDonVi}
                            onChange={(e) => setFilters({ ...filters, tenDonVi: e.target.value })}
                            placeholder="Nhập tên đơn vị"
                        />
                    </div>
                </div>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" size="sm" onClick={onClose}>
                    Hủy
                </Button>
                <Button variant="success" size="sm" onClick={handleExport}>
                    <i className="bi bi-file-earmark-excel me-1"></i> Xuất Excel
                </Button>
            </Modal.Footer>
        </Modal>
    )
}

export default ExportExcelModal
