import { FC } from 'react'
import { Field, Formik, Form } from 'formik'
import { Button } from 'react-bootstrap'

type Props = {
  onFilter: (values: any) => void
  onReset: () => void
}

const UnitsFilter: FC<Props> = ({ onFilter, onReset }) => {
  return (
    <Formik
      initialValues={{ linhVucId: '', tenDonVi: '' }}
      onSubmit={(values) => onFilter(values)}
    >
      {() => (
        <Form className='row g-3 align-items-center'>
          <div className='col-auto'>
            <Field as='select' name='linhVucId' className='form-select'>
              <option value=''>-- Chọn lĩnh vực --</option>
              <option value='1'>Giáo dục</option>
              <option value='2'>Y tế</option>
            </Field>
          </div>
          <div className='col-auto'>
            <Field
              name='tenDonVi'
              className='form-control'
              placeholder='Nhập tên đơn vị'
            />
          </div>
          <div className='col-auto'>
            <Button type='submit'>Search</Button>
            <Button
              variant='light'
              className='ms-2'
              data-bs-toggle='modal'
              data-bs-target='#kt_modal_1'
            >
              Add
            </Button>
          </div>
        </Form>
      )}
    </Formik>
  )
}

export default UnitsFilter
