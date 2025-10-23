import React, { useEffect, useState } from 'react'
import { fetchDonViDetail } from './settings/_requests'

type BienChe = {
    id: string
    tenDonVi: string
    soQuyetDinh: string
    slVienChuc: number
    slHopDong: number
    slHopDongND: number
    slBoTri: number
    slGiaoVien: number
    slQuanLy: number
    slNhanVien: number
    slHD111: number
    effectiveDate: string
}

type BienCheHistory = {
    id: string
    bienCheId: string
    soQuyetDinh: string
    slVienChuc: number
    slHopDong: number
    slHopDongND: number
    slBoTri: number
    slGiaoVien: number
    slQuanLy: number
    slNhanVien: number
    slHD111: number
    effectiveDate: string
    createdAt: string
}

type Props = {
    bienCheId: string | null
    onClose: () => void
}

const BienCheDetailModal: React.FC<Props> = ({ bienCheId, onClose }) => {
    const [current, setCurrent] = useState<BienChe | null>(null)
    const [history, setHistory] = useState<BienCheHistory[]>([])
    const [loading, setLoading] = useState(false)

    // Fetch data khi mở modal
    useEffect(() => {
        if (!bienCheId) return
        setLoading(true)
        setCurrent(null)
        setHistory([])

        fetchDonViDetail(bienCheId)
            .then((res) => {
                setCurrent(res.data.current)
                setHistory(res.data.history)
            })
            .finally(() => setLoading(false))
    }, [bienCheId])

    // Không render nếu modal chưa được mở
    if (!bienCheId) return null

    return (
        <div
            className="modal fade show d-block"
            tabIndex={-1}
            role="dialog"
            style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}
        >
            <div className="modal-dialog modal-xl">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{current?.tenDonVi || 'Chi tiết đơn vị'}</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>

                    <div className="modal-body">
                        {loading && <p>Đang tải dữ liệu...</p>}

                        {!loading && current && (
                            <>
                                {/* Thông tin hiện tại */}
                                <h6>Thông tin hiện tại</h6>
                                <div className="table-responsive mb-4">
                                    <table className="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Số quyết định</th>
                                                <th>Ngày hiệu lực</th>
                                                <th>Viên chức</th>
                                                <th>Hợp đồng</th>
                                                <th>Hợp đồng ND</th>
                                                <th>Bố trí</th>
                                                <th>Giáo viên</th>
                                                <th>Quản lý</th>
                                                <th>Nhân viên</th>
                                                <th>HD111</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>{current.soQuyetDinh}</td>
                                                <td>{new Date(current.effectiveDate).toLocaleDateString()}</td>
                                                <td>{current.slVienChuc}</td>
                                                <td>{current.slHopDong}</td>
                                                <td>{current.slHopDongND}</td>
                                                <td>{current.slBoTri}</td>
                                                <td>{current.slGiaoVien}</td>
                                                <td>{current.slQuanLy}</td>
                                                <td>{current.slNhanVien}</td>
                                                <td>{current.slHD111}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <hr />

                                {/* Lịch sử */}
                                <h6>Lịch sử cập nhật</h6>
                                {history.length > 0 ? (
                                    <div
                                        className="table-responsive"
                                        style={{ maxHeight: '300px', overflowY: 'auto' }}
                                    >
                                        <table className="table table-striped table-bordered">
                                            <thead
                                                style={{
                                                    position: 'sticky',
                                                    top: 0,
                                                    background: '#f8f9fa',
                                                    zIndex: 2,
                                                }}
                                                className="table-light"
                                            >
                                                <tr>
                                                    <th>Ngày hiệu lực</th>
                                                    <th>Số quyết định</th>
                                                    <th>Viên chức</th>
                                                    <th>Hợp đồng</th>
                                                    <th>Hợp đồng ND</th>
                                                    <th>Bố trí</th>
                                                    <th>Giáo viên</th>
                                                    <th>Quản lý</th>
                                                    <th>Nhân viên</th>
                                                    <th>HD111</th>
                                                    <th>Ngày lưu</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {history.map((h) => (
                                                    <tr key={h.id}>
                                                        <td>{new Date(h.effectiveDate).toLocaleDateString()}</td>
                                                        <td>{h.soQuyetDinh}</td>
                                                        <td>{h.slVienChuc}</td>
                                                        <td>{h.slHopDong}</td>
                                                        <td>{h.slHopDongND}</td>
                                                        <td>{h.slBoTri}</td>
                                                        <td>{h.slGiaoVien}</td>
                                                        <td>{h.slQuanLy}</td>
                                                        <td>{h.slNhanVien}</td>
                                                        <td>{h.slHD111}</td>
                                                        <td>{new Date(h.createdAt).toLocaleString()}</td>
                                                    </tr>
                                                ))}
                                            </tbody>
                                        </table>
                                    </div>
                                ) : (
                                    <p>Chưa có lịch sử</p>
                                )}
                            </>
                        )}
                    </div>

                    <div className="modal-footer">
                        <button type="button" className="btn btn-light" onClick={onClose}>
                            Đóng
                        </button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default BienCheDetailModal
