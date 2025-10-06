import React, { useEffect, useState} from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import UnitsFilter from './components/UnitsFilter'
import UnitsTable, { LinhVuc } from './components/UnitsTable'
import { DonVi } from './components/settings/_models'
import { createDonVi, deleteDonVi, fetchDonVis, fetchLinhVucs } from './components/settings/_requests'
import AddUnitModal from './components/AddUnitModal'
import Swal from 'sweetalert2'

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
  const handleDelete = async (unit: DonVi) => {

    if (!unit) return
    const result = await Swal.fire({
      title: 'Bạn có chắc muốn xóa?',
      text: `Hành động này sẽ xóa đơn vị "${unit.tenDonVi}" và không thể hoàn tác!`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Có, xóa!',
      cancelButtonText: 'Hủy'
    })

    if (result.isConfirmed) {
      try {
        await deleteDonVi(unit.id)
        setData(prev => prev.filter(item => item.id !== unit.id))
        Swal.fire('Đã xóa!', 'Bản ghi đã được xóa.', 'success')
      } catch (err) {
        console.error(err)
        Swal.fire('Thất bại!', 'Xóa không thành công.', 'error')
      }
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
    loadData()
  }, [page, pageSize])

  return (
    <>
      <PageTitle breadcrumbs={biencheBreadCrumbs}>Danh Sách Đơn Vị</PageTitle>
      <div className='card'>
        <div className='card-body'>
          <UnitsFilter onFilter={() => { }} onReset={() => { }} />
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
            />
          )}
        </div>
      </div>
      <AddUnitModal
        onSubmit={handleAddUnit}
        onSubmitSuccess={() => console.log('Thêm đơn vị thành công, refresh table')}
      />
    </>
  )
}

export default UnitsPage
