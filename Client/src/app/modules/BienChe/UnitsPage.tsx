import React, { useEffect, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import UnitsFilter from './components/UnitsFilter'
import UnitsTable, { Khoi, LinhVuc } from './components/UnitsTable'
import { DonVi } from './components/settings/_models'
import { createDonVi, deleteDonVi, fetchDonVis, fetchKhois, fetchLinhVucs, searchBienChes, } from './components/settings/_requests'
import AddUnitModal from './components/AddUnitModal'
import Swal from 'sweetalert2'
import { Modal } from 'bootstrap'

const biencheBreadCrumbs: Array<PageLink> = [
  {
    title: 'Biên Chế',
    path: '/bien-che/ds-don-vi',
    isSeparator: false,
    isActive: false,
  },
]

const UnitsPage: React.FC = () => {
  const [data, setData] = useState<LinhVuc[]>([])
  const [loading, setLoading] = useState(false)
  const [page, setPage] = useState(1)
  const [pageSize] = useState(5)
  const [totalRecords, setTotalRecords] = useState(0)
  const [mode, setMode] = useState<'add' | 'edit'>('add')
  const [selectedRecord, setSelectedRecord] = useState<any | null>(null)
  const [linhVucs, setLinhVucs] = useState<LinhVuc[]>([])
  const [khois, setKhois] = useState<Khoi[]>([])


  const handleDelete = async (id: string) => {
    const result = await Swal.fire({
      title: 'Bạn có chắc muốn xóa?',
      text: `Hành động này sẽ xóa bản ghi Biên chế và không thể hoàn tác!`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Có, xóa!',
      cancelButtonText: 'Hủy'
    })

    if (result.isConfirmed) {
      try {
        // gọi API xóa Biên chế
        await deleteDonVi(id)

        // Cập nhật state sau khi xóa
        setData(prev =>
          prev.map(linhVuc => ({
            ...linhVuc,
            khois: linhVuc.khois.map(khoi => ({
              ...khoi,
              bienChes: khoi.bienChes.filter(bc => bc.id !== id)
            }))
          }))
        )

        Swal.fire('Đã xóa!', 'Bản ghi đã được xóa.', 'success')
      } catch (err) {
        console.error(err)
        Swal.fire('Thất bại!', 'Xóa không thành công.', 'error')
      }
    }
  }
  useEffect(() => {
    const fetchData = async () => {
      const lv = await fetchLinhVucs()
      const k = await fetchKhois()
      setLinhVucs(lv.data)
      setKhois(k.data)
    }
    fetchData()
  }, [])
  const handleEdit = (record: any) => {
    setSelectedRecord(record)
    setMode('edit')

    // mở modal
    const modalEl = document.getElementById('kt_modal_1')
    if (modalEl) {
      const modal = new Modal(modalEl)
      modal.show()
    }
  }

  const loadData = async () => {
    try {
      setLoading(true)
      const res = await fetchDonVis(page, pageSize)
      setData(res.data.items)

      setTotalRecords(res.data.totalRecords)
    } catch (err) {
      console.error('Lỗi khi load Linh Vự:', err)
    } finally {
      setLoading(false)
    }
  }
  const handleAddUnit = async (data: any) => {
    try {
      const newUnit = await createDonVi(data)
      setData(prev => [...prev, newUnit])
      alert('Thêm đơn vị thành công!')
    } catch (err) {
      console.error(err)
      alert('Thêm đơn vị thất bại!')
    }
  }
  useEffect(() => {
    console.log('Request page:', page, 'pageSize:', pageSize);

    loadData()
  }, [page, pageSize])
  const handleFilter = async (values: any) => {
    // nếu không nhập gì thì lấy all
    if (!values.tenDonVi && !values.linhVucId && !values.khoiId) {
      loadData()
    } else {
      const res = await searchBienChes(values)
      setData(res.items)
      setTotalRecords(res.totalRecords)   // cần set lại
      
    }
  }
  return (
    <>
      <PageTitle breadcrumbs={biencheBreadCrumbs}>Danh Sách Đơn Vị</PageTitle>
      <div className='card'>
        <div className='card-body'>
          <UnitsFilter onFilter={handleFilter} linhVucs={linhVucs} khois={khois} onReset={() => { }} />
          <div className='separator my-6'></div>
          {loading ? (
            <p>Đang tải dữ liệu...</p>
          ) : (
            <UnitsTable
              data={data}
              totalRecords={totalRecords}
              page={page}
              pageSize={pageSize}
              onPageChange={setPage}
              onDelete={handleDelete}
              onUpdate={handleEdit}
            />
          )}
        </div>
      </div>
      <AddUnitModal
        onSubmit={handleAddUnit}
        onSubmitSuccess={loadData}
        initialData={selectedRecord}
        mode={mode}
        linhVucs={linhVucs} khois={khois}
      />
    </>
  )
}

export default UnitsPage
