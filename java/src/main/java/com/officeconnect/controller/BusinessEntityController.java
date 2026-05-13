package com.officeconnect.controller;

import com.officeconnect.dto.*;
import com.officeconnect.service.EmployeeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/BusinessEntity")
public class BusinessEntityController {

    @Autowired
    private EmployeeService employeeService;

    // =================== Company ===================

    @PostMapping("/DDCompany")
    public ResponseEntity<?> ddCompany(@RequestBody DDCompanyViewModel model) {
        try {
            List<DDCompanyViewModel> result = employeeService.getDDCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetAllCompany")
    public ResponseEntity<?> getAllCompany(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetCompany")
    public ResponseEntity<?> getCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.getCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddCompany")
    public ResponseEntity<?> addCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateCompany")
    public ResponseEntity<?> updateCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteCompany")
    public ResponseEntity<?> deleteCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/ActivateCompany")
    public ResponseEntity<?> activateCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.activateCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeActivateCompany")
    public ResponseEntity<?> deActivateCompany(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deActivateCompany(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    // =================== Legal Entity ===================

    @PostMapping("/DDLegalEntity")
    public ResponseEntity<?> ddLegalEntity(@RequestBody DDLegalEntityViewModel model) {
        try {
            List<DDLegalEntityViewModel> result = employeeService.getDDLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetAllLegalEntity")
    public ResponseEntity<?> getAllLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetLegalEntity")
    public ResponseEntity<?> getLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.getLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddLegalEntity")
    public ResponseEntity<?> addLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateLegalEntity")
    public ResponseEntity<?> updateLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteLegalEntity")
    public ResponseEntity<?> deleteLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/ActivateLegalEntity")
    public ResponseEntity<?> activateLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.activateLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeActivateLegalEntity")
    public ResponseEntity<?> deActivateLegalEntity(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deActivateLegalEntity(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    // =================== Business Unit ===================

    @PostMapping("/DDBusinessUnit")
    public ResponseEntity<?> ddBusinessUnit(@RequestBody DDBusinessUnitViewModel model) {
        try {
            List<DDBusinessUnitViewModel> result = employeeService.getDDBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetAllBusinessUnit")
    public ResponseEntity<?> getAllBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetBusinessUnit")
    public ResponseEntity<?> getBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.getBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddBusinessUnit")
    public ResponseEntity<?> addBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateBusinessUnit")
    public ResponseEntity<?> updateBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteBusinessUnit")
    public ResponseEntity<?> deleteBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/ActivateBusinessUnit")
    public ResponseEntity<?> activateBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.activateBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeActivateBusinessUnit")
    public ResponseEntity<?> deActivateBusinessUnit(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deActivateBusinessUnit(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    // =================== Calendar Year ===================

    @PostMapping("/GetAllCalendarYear")
    public ResponseEntity<?> getAllCalendarYear(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllCalendarYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetCalendarYear")
    public ResponseEntity<?> getCalendarYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.getCalendarYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddCalendarYear")
    public ResponseEntity<?> addCalendarYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addCalendarYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateCalendarYear")
    public ResponseEntity<?> updateCalendarYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateCalendarYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteCalendarYear")
    public ResponseEntity<?> deleteCalendarYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteCalendarYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    // =================== Financial Year ===================

    @PostMapping("/GetAllFinancialYear")
    public ResponseEntity<?> getAllFinancialYear(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllFinancialYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetFinancialYear")
    public ResponseEntity<?> getFinancialYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.getFinancialYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddFinancialYear")
    public ResponseEntity<?> addFinancialYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addFinancialYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateFinancialYear")
    public ResponseEntity<?> updateFinancialYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateFinancialYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteFinancialYear")
    public ResponseEntity<?> deleteFinancialYear(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteFinancialYear(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    // =================== Location ===================

    @PostMapping("/DDLocation")
    public ResponseEntity<?> ddLocation(@RequestBody DDLocationViewModel model) {
        try {
            List<DDLocationViewModel> result = employeeService.getDDLocation(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/GetAllLocation")
    public ResponseEntity<?> getAllLocation(@RequestBody Map<String, Object> model) {
        try {
            List<Map<String, Object>> result = employeeService.getAllLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/AddLocation")
    public ResponseEntity<?> addLocation(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.addLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/UpdateLocation")
    public ResponseEntity<?> updateLocation(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.updateLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeleteLocation")
    public ResponseEntity<?> deleteLocation(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deleteLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/ActivateLocation")
    public ResponseEntity<?> activateLocation(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.activateLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }

    @PostMapping("/DeActivateLocation")
    public ResponseEntity<?> deActivateLocation(@RequestBody Map<String, Object> model) {
        try {
            Map<String, Object> result = employeeService.deActivateLocationBE(model);
            return ResponseEntity.ok(result);
        } catch (RuntimeException ex) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(Map.of("StatusCode", 404, "Message", ex.getMessage()));
        }
    }
}
