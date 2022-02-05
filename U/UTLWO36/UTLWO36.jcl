#UTLWO36.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO36.LWO36R1
   then
        rm ${reportdir}/ULWO36.LWO36R1
fi
if test -a ${reportdir}/ULWO36.LWO36R2
   then
        rm ${reportdir}/ULWO36.LWO36R2
fi
if test -a ${reportdir}/ULWO36.LWO36R3
   then
        rm ${reportdir}/ULWO36.LWO36R3
fi
if test -a ${reportdir}/ULWO36.LWO36R4
   then
        rm ${reportdir}/ULWO36.LWO36R4
fi
if test -a ${reportdir}/ULWO36.LWO36R5
   then
        rm ${reportdir}/ULWO36.LWO36R5
fi
if test -a ${reportdir}/ULWO36.LWO36R6
   then
        rm ${reportdir}/ULWO36.LWO36R6
fi
if test -a ${reportdir}/ULWO36.LWO36R7
   then
        rm ${reportdir}/ULWO36.LWO36R7
fi
if test -a ${reportdir}/ULWO36.LWO36RZ
   then
        rm ${reportdir}/ULWO36.LWO36RZ
fi

# run the program

sas ${codedir}/UTLWO36.sas -log ${reportdir}/ULWO36.LWO36R1  -mautosource
